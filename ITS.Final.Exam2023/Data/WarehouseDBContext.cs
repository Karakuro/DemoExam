using Microsoft.EntityFrameworkCore;

namespace ITS.Final.Exam2023.Data
{
    public class WarehouseDBContext(DbContextOptions<WarehouseDBContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
    }
}
