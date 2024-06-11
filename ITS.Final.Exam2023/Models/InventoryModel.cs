using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ITS.Final.Exam2023.Models
{
    public class InventoryModel
    {
        [MaxLength(20), MinLength(5)]
        [RegularExpression(@"^[A-Za-z0-9]+$")]
        public required string Code { get; set; }
        [Range(0, int.MaxValue)]
        public required int Quantity { get; set; }
    }
}
