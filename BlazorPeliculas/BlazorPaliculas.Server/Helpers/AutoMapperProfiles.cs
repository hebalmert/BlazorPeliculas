using AutoMapper;
using BlazorPeliculas.Shared.Entidades;

namespace BlazorPaliculas.Server.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            //para que haga un mapeo de acto a actor pero que ignore el campo FOTO
            CreateMap<Actor, Actor>().ForMember(x => x.Foto, option => option.Ignore());

            //para que haga un mapeo de acto a actor pero que ignore el campo FOTO
            CreateMap<Peliculas, Peliculas>().ForMember(x => x.Poster, option => option.Ignore());
        }
    }
}
