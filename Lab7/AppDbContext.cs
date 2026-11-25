using LabTiu7.Models;
using Microsoft.EntityFrameworkCore;

namespace LabTiu7.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Laptop> Laptopy { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Laptop>().HasData(
                new Laptop { Id = 1, Producent = "Lenovo", Model = "Legion 5 Pro", PamiecRamGB = 16, Cena = 5499.99m, CzyDostepny = true },
                new Laptop { Id = 2, Producent = "Apple", Model = "MacBook Air M2", PamiecRamGB = 8, Cena = 4899.00m, CzyDostepny = true },
                new Laptop { Id = 3, Producent = "Dell", Model = "XPS 15", PamiecRamGB = 32, Cena = 8999.50m, CzyDostepny = false },
                new Laptop { Id = 4, Producent = "Asus", Model = "ROG Strix G16", PamiecRamGB = 16, Cena = 6299.00m, CzyDostepny = true }
            );
        }
    }
}
