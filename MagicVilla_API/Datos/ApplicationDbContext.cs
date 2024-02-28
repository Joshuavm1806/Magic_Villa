using MagicVilla_API.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<NumeroVilla> NumeroVillas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                    new Villa()
                    {
                        id = 1,
                        Nombre = "Villa deluxe",
                        Ocupantes = 4,
                        MetrosCuadrados = 120,
                        Detalle = "Amplia Villa para pasarla bien",
                        Tarifa = 150,
                        FechaCreacion = DateTime.Now,
                        FechaActualizacion = DateTime.Now,
                        Amenidad = "",
                        ImagenUrl = ""
                    },
                     new Villa()
                     {
                         id = 2,
                         Nombre = "Villa premium",
                         Ocupantes = 8,
                         MetrosCuadrados = 150,
                         Detalle = "Amplia Villa con vista al mar",
                         Tarifa = 350,
                         FechaCreacion = DateTime.Now,
                         FechaActualizacion = DateTime.Now,
                         Amenidad = "",
                         ImagenUrl = ""
                     }
           );
        }
    }
}
