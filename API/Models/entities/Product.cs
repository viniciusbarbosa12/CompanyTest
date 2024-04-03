using Models.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.entities
{
    [Table(nameof(Product))]
    public class Product : BaseEntity
    {
        public Product()
        {
        }


        [Required]
        [StringLength(100)]
        public string? Name { get; set; }


        [Required]
        [StringLength(300)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public Guid CategoryId { get; set; }

        [JsonIgnore]
        public Category? Category { get; set; }
    }
}
