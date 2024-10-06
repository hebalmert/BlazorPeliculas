using BlazorPeliculas.Shared.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPeliculas.Shared.DTOs
{
    public class PeliculaVisualizarDTO
    {
        public Peliculas Pelicula { get; set; } = null!;

        public List<GeneroCls> Generos { get; set; } = new List<GeneroCls>();

        public List<Actor> Actores { get; set; } = new List<Actor>();

        public int VotoUsuario { get; set; }

        public double PromedioVotos { get; set; }
    }
}
