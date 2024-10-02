using BlazorPeliculas.Shared.Entidades;

namespace BlazorPeliculas.Client.Repositorios
{
    public class Repositorio : IRepositorio
    {
        public List<Peliculas> ObtenerPeliculas()
        {
            return new List<Peliculas>
                {
                     new Peliculas{ Titulo="Rocky", FechaLanzamiento = new DateTime(2024, 9, 20),
                        Poster="https://m.media-amazon.com/images/I/51KBSyprSRL._AC_UF894,1000_QL80_.jpg"},
                     new Peliculas{ Titulo="Moana", FechaLanzamiento = new DateTime(2016, 11, 23),
                        Poster="https://m.media-amazon.com/images/I/81rjqvHFtkL.jpg"},
                     new Peliculas{ Titulo="Inception", FechaLanzamiento = new DateTime(2010, 7, 16),
                        Poster = "https://m.media-amazon.com/images/I/71uKM+LdgFL._AC_UF894,1000_QL80_.jpg"}
                };
        }
    }
}
