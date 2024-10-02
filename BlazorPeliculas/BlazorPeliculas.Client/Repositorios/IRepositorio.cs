using BlazorPeliculas.Shared.Entidades;

namespace BlazorPeliculas.Client.Repositorios
{
    public interface IRepositorio
    {
        List<Peliculas> ObtenerPeliculas();
    }
}
