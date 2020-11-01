using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Services.Data
{
    public class PetShopContextFactory : IDesignTimeDbContextFactory<PetShopContext>
    {
        public PetShopContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PetShopContext>();
            var connectionString = Environment.GetEnvironmentVariable("PetStoreConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Invalid connection string!");
            }

            optionsBuilder.UseSqlServer(connectionString);

            return new PetShopContext(optionsBuilder.Options);
        }
    }
}