using Microsoft.EntityFrameworkCore;

namespace ITS.Final.Exam2023.Data
{
    //public class WarehouseDBContext(DbContextOptions<WarehouseDBContext> options) : DbContext(options)
    public class WarehouseDBContext : DbContext
    {
        public WarehouseDBContext(DbContextOptions<WarehouseDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //DA USARE SOLO SE I DATI DEVONO ESSERE INSERITI UNA TANTUM COME SETUP INIZIALE
            modelBuilder.Entity<Product>().HasData(new List<Product>() { new Product() {
                ProductId = "TestProduct",
                Description = "TestDescription"
            } 
            });
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
    }
}
