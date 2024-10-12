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
    [Route("api/actores")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAlmacenadorArchivos _almacenadorArchivos;
        private readonly IMapper _mapper;
        private readonly string contenedor = "personas";
        private readonly string contenedorLocal = "wwwroot\\Images\\ImgActor";

        public ActoresController(ApplicationDbContext context,
            IAlmacenadorArchivos almacenadorArchivos, IMapper mapper)
        {
            _context = context;
            _almacenadorArchivos = almacenadorArchivos;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> Get([FromQuery] PaginacionDTO paginacion)
        {
            var queryable = _context.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacion.CantidadRegistros);

            return await queryable.OrderBy(x => x.Nombre).Paginar(paginacion).ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Actor>> Get(int id)
        {
            var modelo = await _context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            if (modelo is null)
            {
                return NotFound();
            }
            return modelo!;
        }


        [HttpGet("buscar/{textBusqueda}")]
        public async Task<ActionResult<List<Actor>>> Get(string textBusqueda)
        {
            if (string.IsNullOrWhiteSpace(textBusqueda))
            {
                return new List<Actor>();
            }

            return await _context.Actores.
                Where(x => x.Nombre.ToLower().Contains(textBusqueda.ToLower())).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Actor modelo)
        {
            if (!string.IsNullOrWhiteSpace(modelo.Foto))
            {
                var fotoActor = Convert.FromBase64String(modelo.Foto);
                modelo.Foto = await _almacenadorArchivos.SaveFileAsync(fotoActor, ".jpg", contenedor);
                //modelo.Foto = await _almacenadorArchivos.UploadImage(fotoActor, ".jpg", contenedorLocal);

            }

            _context.Add(modelo);
            await _context.SaveChangesAsync();

            return modelo.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Actor modelo)
        {
            var actorDB = await _context.Actores.FirstOrDefaultAsync(x => x.Id == modelo.Id);
            if (actorDB is null)
            {
                return NotFound();
            }

            //Dice que los datos que vienen en Modelo los actualice en actorDB para Actualizar el registro
            actorDB = _mapper.Map(modelo, actorDB);

            if (!string.IsNullOrWhiteSpace(modelo.Foto))
            {
                var fotoActor = Convert.FromBase64String(modelo.Foto);
                actorDB.Foto = await _almacenadorArchivos.EditFileAsync(fotoActor, ".jpg", contenedor, modelo.Foto);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var actor = await _context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            if (actor is null)
            {
                return NotFound();
            }

            _context.Remove(actor);
            await _context.SaveChangesAsync();

            _almacenadorArchivos.DeleteImage(actor.Foto!, contenedor);

            return NoContent();
        }
    }
}
