using AutoMapper;
using BlazorPaliculas.Server.Helpers;
using BlazorPeliculas.Shared.DTOs;
using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorPaliculas.Server.Controllers
{
    [Route("api/peliculas")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAlmacenadorArchivos _almacenadorArchivos;
        private readonly IMapper _mapper;
        private readonly string contenedor = "peliculas";
        private readonly string contenedorLocal = "wwwroot\\Images\\ImgActor";

        public PeliculasController(ApplicationDbContext context,
            IAlmacenadorArchivos almacenadorArchivos, IMapper mapper)
        {
            _context = context;
            _almacenadorArchivos = almacenadorArchivos;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<HomePageDTO>> Get()
        {
            var limite = 6;

            var peliculaEnCartelera = await _context.Peliculas.Where(x => x.EnCartelera).Take(limite)
                .OrderByDescending(x => x.FechaLanzamiento).ToListAsync();

            var fechaActual = DateTime.Today;

            var proximosEstrenos = await _context.Peliculas.Where(x => x.FechaLanzamiento > fechaActual)
                .OrderBy(x => x.FechaLanzamiento).Take(limite).ToListAsync();

            var resultado = new HomePageDTO()
            {
                PeliculasEnCartelera = peliculaEnCartelera,
                ProximosEstrenos = proximosEstrenos
            };


            return resultado;
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<PeliculaVisualizarDTO>> GetPelicula(int id)
        {
            var pelicula = await _context.Peliculas
                .Include(x => x.GenerosPelicula).ThenInclude(x => x.Genero)
                .Include(x => x.PeliculasActor.OrderBy(p => p.Orden)).ThenInclude(x => x.Actor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pelicula is null)
            {
                return NotFound();
            }

            //TODO: Sistema de Votacion
            var promedioVoto = 4;
            var votoUsuario = 5;

            var Nmodelo = new PeliculaVisualizarDTO();
            Nmodelo.Pelicula = pelicula;
            Nmodelo.Generos = pelicula.GenerosPelicula.Select(gp => gp.Genero).ToList()!;
            Nmodelo.Actores = pelicula.PeliculasActor.Select(pa => new Actor
            {
                Nombre = pa.Actor!.Nombre,
                Foto = pa.Actor.Foto,
                Personaje = pa.Personaje,
                Id = pa.ActorId
            }).ToList();

            Nmodelo.PromedioVotos = promedioVoto;
            Nmodelo.VotoUsuario = votoUsuario;

            return Nmodelo;
        }

        [HttpGet("filtrar")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Peliculas>>> Get([FromQuery] ParametrosBusquedaPeliculasDTO modelo)
        {
            var peliculasQueryable = _context.Peliculas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(modelo.Titulo))
            {
                peliculasQueryable = peliculasQueryable.Where(x => x.Titulo.Contains(modelo.Titulo));
            }
            if (modelo.EnCartelera)
            {
                peliculasQueryable = peliculasQueryable.Where(x => x.EnCartelera);
            }
            if (modelo.Estrenos)
            {
                var hoy = DateTime.Today;
                peliculasQueryable = peliculasQueryable.Where(x => x.FechaLanzamiento >= hoy);
            }
            if (modelo.GeneroId != 0)
            {
                peliculasQueryable = peliculasQueryable
                    .Where(x => x.GenerosPelicula.Select(y => y.GeneroId).Contains(modelo.GeneroId));
            }

            //TODO: Sistema de votacion

            await HttpContext.InsertarParametrosPaginacionEnRespuesta(peliculasQueryable, modelo.CantidadRegistros);

            var peliculas = await peliculasQueryable.Paginar(modelo.PaginacionDTO).ToListAsync();
            return peliculas;
        }


        [HttpGet("actualizar/{id:int}")]
        public async Task<ActionResult<PeliculaActualizacionDTO>> PutGet(int id)
        {
            var peliculaActionResult = await GetPelicula(id);
            if (peliculaActionResult.Result is NotFoundResult) { return NotFound(); }

            var peliculaParaElDTO = peliculaActionResult.Value;
            var generosSeleccionadosIds = peliculaParaElDTO!.Generos.Select(x => x.Id).ToList();
            var generosNoSeleccionados = await _context.Generos.Where(x => !generosSeleccionadosIds.Contains(x.Id)).ToListAsync();

            var modelo = new PeliculaActualizacionDTO();
            modelo.Pelicula = peliculaParaElDTO.Pelicula;
            modelo.GenerosNoSeleccionados = generosNoSeleccionados;
            modelo.GenerosSeleccionados = peliculaParaElDTO.Generos;
            modelo.Actores = peliculaParaElDTO.Actores;

            return modelo;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Peliculas modelo)
        {
            if (!string.IsNullOrWhiteSpace(modelo.Poster))
            {
                var poster = Convert.FromBase64String(modelo.Poster);
                modelo.Poster = await _almacenadorArchivos.SaveFileAsync(poster, ".jpg", contenedor);
                //modelo.Foto = await _almacenadorArchivos.UploadImage(fotoActor, ".jpg", contenedorLocal);

            }

            EscribirOrdenActores(modelo);

            _context.Add(modelo);
            await _context.SaveChangesAsync();

            return modelo.Id;
        }

        private static void EscribirOrdenActores(Peliculas modelo)
        {
            if (modelo.PeliculasActor is not null)
            {
                for (int i = 0; i < modelo.PeliculasActor.Count; i++)
                {
                    modelo.PeliculasActor[i].Orden = i + 1;
                }
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put(Peliculas modelo)
        {
            var peliculaDb = await _context.Peliculas
                .Include(x => x.GenerosPelicula)
                .Include(x => x.PeliculasActor)
                .FirstOrDefaultAsync(x => x.Id == modelo.Id);
            if (peliculaDb is null)
            {
                return NotFound();
            }

            peliculaDb = _mapper.Map(modelo, peliculaDb);

            if (!string.IsNullOrWhiteSpace(modelo.Poster))
            {
                var posterImagen = Convert.FromBase64String(modelo.Poster);
                peliculaDb.Poster = await _almacenadorArchivos.EditFileAsync(posterImagen, ".jpg", contenedor, modelo.Poster);
            }

            EscribirOrdenActores(peliculaDb);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var peli = await _context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);
            if (peli is null)
            {
                return NotFound();
            }

            _context.Remove(peli);
            await _context.SaveChangesAsync();

            _almacenadorArchivos.DeleteImage(peli.Poster!, contenedor);

            return NoContent();
        }
    }
}
