using Microsoft.EntityFrameworkCore;
using Services.Models;

namespace Services.Data
{
    public class PetShopContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }

        public PetShopContext(DbContextOptions<PetShopContext> options)
            : base(options)
        {}
    }
}