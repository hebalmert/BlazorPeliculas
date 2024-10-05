using Microsoft.VisualBasic;

namespace BlazorPeliculas.Shared.Entidades
{
    public class VotoPelicula
    {
        public int Id { get; set; }

        public int Voto { get; set; }   

        public DateTime FechaVoto { get; set; }

        public int PeliculaId { get; set; }

        public Peliculas? Pelicula { get; set; }
    }
}
