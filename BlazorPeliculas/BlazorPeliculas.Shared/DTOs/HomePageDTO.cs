using BlazorPeliculas.Shared.Entidades;

namespace BlazorPeliculas.Shared.DTOs
{
    public class HomePageDTO
    {
        public List<Peliculas>? PeliculasEnCartelera { get; set; }

        public List<Peliculas>? ProximosEstrenos { get; set; }
    }
}
