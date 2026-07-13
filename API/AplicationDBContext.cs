using FutbolAPI.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FutbolAPI
{
    public class AplicationDBContext : IdentityDbContext
    {
        public AplicationDBContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FechaYhora>().HasKey(e => e.id);
        }
        public DbSet<FechaYhora> Fechas_y_horas { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Cancha> Canchas { get; set; }


    }
}
