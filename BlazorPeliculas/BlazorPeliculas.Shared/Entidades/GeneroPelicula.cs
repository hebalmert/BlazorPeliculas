namespace BlazorPeliculas.Shared.Entidades
{
    public class GeneroPelicula
    {
        public int PeliculaId { get; set; }

        public int GeneroId { get; set; }

        public GeneroCls? Genero { get; set; }

        public Peliculas? Peliculas { get; set; }

    }
}
