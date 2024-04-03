using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.entities
{

    [Table(nameof(Category))]
    public class Category
    {

        public Category()
        {
            Id = Guid.NewGuid();
            Products = new Collection<Product>();
        }

        [Key]
        public Guid Id { get; set; }


        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        
        [Required]
        [StringLength(300)]
        public string? Codigo { get; set; }

        [JsonIgnore]
        public ICollection<Product> Products { get; set; }
    }
}
