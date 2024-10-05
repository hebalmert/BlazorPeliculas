using BlazorPaliculas.Server.Helpers;
using BlazorPeliculas.Shared.DTOs;
using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorPaliculas.Server.Controllers
{
    [Route("api/peliculas")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAlmacenadorArchivos _almacenadorArchivos;
        private readonly string contenedor = "peliculas";
        private readonly string contenedorLocal = "wwwroot\\Images\\ImgActor";

        public PeliculasController(ApplicationDbContext context, IAlmacenadorArchivos almacenadorArchivos)
        {
            _context = context;
            _almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<HomePageDTO>> Get() 
        {
            var limite = 6;
            
            var peliculaEnCartelera = await _context.Peliculas.Where(x=> x.EnCartelera).Take(limite)
                .OrderByDescending(x=> x.FechaLanzamiento).ToListAsync();

            var fechaActual = DateTime.Today;

            var proximosEstrenos = await _context.Peliculas.Where(x=> x.FechaLanzamiento > fechaActual)
                .OrderBy(x=> x.FechaLanzamiento).Take(limite).ToListAsync();

            var resultado = new HomePageDTO() 
            { 
                PeliculasEnCartelera = peliculaEnCartelera,
                ProximosEstrenos = proximosEstrenos
            };


            return resultado;
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

            _context.Add(modelo);
            await _context.SaveChangesAsync();

            return modelo.Id;
        }
    }
}
