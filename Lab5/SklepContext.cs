using Microsoft.EntityFrameworkCore;
using TiuLab5.Models;
namespace TiuLab5.Data
{
    public class SklepContext : DbContext
    {
        public SklepContext(DbContextOptions<SklepContext> options)
            : base(options)
        {
        }

        public DbSet<Laptop> Laptopy { get; set; }
        public DbSet<Podzespol> Podzespoly { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Przykładowe podzespoły
            modelBuilder.Entity<Podzespol>().HasData(
                new Podzespol { PodzespolId = 1, Nazwa = "Intel Core i7" },
                new Podzespol { PodzespolId = 2, Nazwa = "Apple M2" },
                new Podzespol { PodzespolId = 3, Nazwa = "AMD Ryzen 7" }
            );

            // Aktualizacja danych startowych laptopów o przypisane PodzespolId
            modelBuilder.Entity<Laptop>().HasData(
                new Laptop
                {
                    LaptopId = 1,
                    Model = "Dell XPS 13",
                    Ocena = 4.8,
                    Dostepny = true,
                    PodzespolId = 1
                },
                new Laptop
                {
                    LaptopId = 2,
                    Model = "MacBook Air M2",
                    Ocena = 4.9,
                    Dostepny = true,
                    PodzespolId = 2
                },
                new Laptop
                {
                    LaptopId = 3,
                    Model = "Lenovo ThinkPad",
                    Ocena = null,
                    Dostepny = false,
                    PodzespolId = null // brak przypisanego podzespołu (0..1)
                }
            );
        }
    }
}
