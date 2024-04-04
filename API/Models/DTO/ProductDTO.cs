using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Models.DTO
{
    public class ProductDTO
    {
        public string? Name { get; set; }

        public Guid? Id { get; set; }
        public string? Description { get; set; }

        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }

    }
}
