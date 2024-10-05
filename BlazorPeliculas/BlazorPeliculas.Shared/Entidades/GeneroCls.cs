using System.ComponentModel.DataAnnotations;

namespace BlazorPeliculas.Shared.Entidades
{
    public class GeneroCls
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        public string? Nombre { get; set; }

        public List<GeneroPelicula>? GenerosPelicula { get; set; } = new List<GeneroPelicula>();
    }
}
