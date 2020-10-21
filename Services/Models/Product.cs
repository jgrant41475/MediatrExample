using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.0, 1000.0)]
        public decimal Price { get; set; }

        public bool IsAnimal { get; set; }
    }
}