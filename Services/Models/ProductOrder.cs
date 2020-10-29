using System;

namespace Services.Models
{
    public class ProductOrder
    {
        public Guid Id { get; set; }

        public int Quantity { get; set; }
        
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }

        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}