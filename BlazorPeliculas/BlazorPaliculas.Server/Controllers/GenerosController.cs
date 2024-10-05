using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorPaliculas.Server.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GenerosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeneroCls>>> Get() 
        {
            return await _context.Generos.ToListAsync();
        }


        [HttpPost]
        public async Task<ActionResult<int>> Post(GeneroCls genero)
        {
            _context.Add(genero);
            await _context.SaveChangesAsync();

            return genero.Id;
        }
    }
}
