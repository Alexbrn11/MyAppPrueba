using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyApp.Entities.Models;

namespace MyApp.DataAccess
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Índice único en el código del artículo
            builder.Entity<Articulo>()
                .HasIndex(a => a.Codigo)
                .IsUnique();

            // Relación: Prestamo -> Usuario
            builder.Entity<Prestamo>()
                .HasOne(p => p.Usuario)
                .WithMany() // o WithMany(u => u.Prestamos) si lo defines en ApplicationUser
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación: Prestamo -> Articulo
            builder.Entity<Prestamo>()
                .HasOne(p => p.Articulo)
                .WithMany()
                .HasForeignKey(p => p.ArticuloId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
