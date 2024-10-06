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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GeneroCls>> Get(int id)
        {
            var modelo = await _context.Generos.FirstOrDefaultAsync(x => x.Id == id);
            if (modelo is null)
            {
                return NotFound();
            }
            return modelo!;
        }


        [HttpPost]
        public async Task<ActionResult<int>> Post(GeneroCls genero)
        {
            _context.Add(genero);
            await _context.SaveChangesAsync();

            return genero.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(GeneroCls modelo)
        {
            _context.Update(modelo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
