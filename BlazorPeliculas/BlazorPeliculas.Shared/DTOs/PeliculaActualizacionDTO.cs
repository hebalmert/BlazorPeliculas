using BlazorPeliculas.Shared.Entidades;

namespace BlazorPeliculas.Shared.DTOs
{
    public class PeliculaActualizacionDTO
    {
        public Peliculas? Pelicula { get; set; }

        public List<Actor> Actores { get; set; } = new List<Actor>();

        public List<GeneroCls> GenerosSeleccionados { get; set; } = new List<GeneroCls>();
        public List<GeneroCls> GenerosNoSeleccionados { get; set; } = new List<GeneroCls>();
    }
}
