using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorPaliculas.Server
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<GeneroCls> Generos => Set<GeneroCls>();

        public DbSet<Actor> Actores => Set<Actor>();

        public DbSet<Peliculas> Peliculas => Set<Peliculas>();

        public DbSet<GeneroPelicula> GenerosPeliculas => Set<GeneroPelicula>();

        public DbSet<PeliculaActor> PeliculasActores => Set<PeliculaActor>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Llave Primaria Compuesta
            modelBuilder.Entity<GeneroPelicula>().HasKey(x => new { x.GeneroId, x.PeliculaId });

            modelBuilder.Entity<PeliculaActor>().HasKey(x => new { x.ActorId, x.PeliculaId });
        }
    }
}
