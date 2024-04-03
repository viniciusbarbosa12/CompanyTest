using Models.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.entities
{

    [Table(nameof(Category))]
    public class Category : BaseEntity
    {

        public Category()
        {
            Products = new Collection<Product>();
        }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        
        [Required]
        [StringLength(300)]
        public string? Code { get; set; }

        [JsonIgnore]
        public ICollection<Product> Products { get; set; }
    }
}
