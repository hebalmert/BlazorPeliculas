using BlazorPaliculas.Server.Helpers;
using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorPaliculas.Server.Controllers
{
    [Route("api/actores")]
    [ApiController]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAlmacenadorArchivos _almacenadorArchivos;
        private readonly string contenedor = "personas";
        private readonly string contenedorLocal = "wwwroot\\Images\\ImgActor";

        public ActoresController(ApplicationDbContext context, IAlmacenadorArchivos almacenadorArchivos)
        {
            _context = context;
            _almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> Get()
        {
            return await _context.Actores.ToListAsync();
        }

        [HttpGet("buscar/{textBusqueda}")]
        public async Task<ActionResult<List<Actor>>> Get(string textBusqueda) 
        {
            if (string.IsNullOrWhiteSpace(textBusqueda))
            {
                return new List<Actor>();
            }
            
            return await _context.Actores.
                Where(x=> x.Nombre.ToLower().Contains(textBusqueda.ToLower())).ToListAsync();
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
    }
}
