namespace BlazorPeliculas.Shared.Entidades
{
    public class PeliculaActor
    {
        public int ActorId { get; set; }

        public int PeliculaId { get; set; }

        public Actor? Actor { get; set; }

        public Peliculas? Pelicula { get; set; }

        public string? Personaje { get; set; }

        public int Orden { get; set; }
    }
}
