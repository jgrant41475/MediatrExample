using System;
using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
    public class Customer
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(120)]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(120)]
        [MinLength(2)]
        public string LastName { get; set; }

        public string Address { get; set; }
        public DateTime CreateDateUtc { get; set; }

#nullable enable
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public DateTime? DeletedDateUtc { get; set; }
#nullable disable
    }
}