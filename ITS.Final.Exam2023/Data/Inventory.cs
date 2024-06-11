using System.ComponentModel.DataAnnotations;

namespace ITS.Final.Exam2023.Data
{
    public class Inventory
    {
        public Guid InventoryId { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        public DateTime Timestamp { get; set; }
        public required string ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
