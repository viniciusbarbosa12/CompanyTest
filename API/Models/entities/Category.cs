using System.Collections.ObjectModel;

namespace Models.entities
{
    public class Category
    {

        public Category()
        {
            Products = new Collection<Product>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Codigo { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
