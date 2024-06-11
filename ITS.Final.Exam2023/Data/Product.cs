using System.ComponentModel.DataAnnotations;

namespace ITS.Final.Exam2023.Data
{
    public class Product
    {
        [MaxLength(20)]
        public string ProductId { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public List<Inventory>? Inventories { get; set; }
    }
}
