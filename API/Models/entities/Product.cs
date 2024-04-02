using System.ComponentModel.DataAnnotations.Schema;

namespace Models.entities
{
    public class Product
    {
        public Guid ProdutoId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
        public decimal Price { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
