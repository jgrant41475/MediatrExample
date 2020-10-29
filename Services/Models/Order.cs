using System;

namespace Services.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public DateTime OrderPlaced { get; set; }

        public Guid CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}