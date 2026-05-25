using InventarioMedicoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioMedicoAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Índice sobre Categoria para acelerar búsquedas frecuentes
            modelBuilder.Entity<Producto>()
                .HasIndex(p => p.Categoria);

            // Índice sobre Activo para filtrar productos activos rápidamente
            modelBuilder.Entity<Producto>()
                .HasIndex(p => p.Activo);


            // Seed: 5000 productos médicos de prueba
            var productos = new List<Producto>();
            var categorias = new[] { "Medicamentos", "Insumos", "Equipos", "Laboratorio", "Emergencia" };
            var random = new Random(42);
            for (int i = 1; i <= 5000; i++)
            {
                productos.Add(new Producto
                {
                    Id = i,
                    Nombre = $"Producto Médico {i}",
                    Categoria = categorias[random.Next(categorias.Length)],
                    Precio = Math.Round((decimal)(random.NextDouble() * 500 + 1), 2),
                    Stock = random.Next(0, 1000),
                    Activo = random.Next(0, 2) == 1,
                    FechaIngreso = DateTime.Now.AddDays(-random.Next(1, 365))
                });
            }

            modelBuilder.Entity<Producto>().HasData(productos);
        }
    }
}
