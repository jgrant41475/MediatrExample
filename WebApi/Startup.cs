using System;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Customers.Queries.ListCustomersQuery;
using Services.Data;

namespace WebApi
{
    public class Startup
    {
        private const string PetShopOrigins = "_PetShopOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: PetShopOrigins,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            
            services.AddHttpContextAccessor();

            services.AddDbContext<PetShopContext>(options =>
            {
                var connectionString = Environment.GetEnvironmentVariable("PetStoreConnectionString");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("Invalid connection string!");
                }

                options.UseSqlServer(connectionString);
            });

            services.AddMediatR(typeof(ListCustomersQuery).Assembly);
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(PetShopOrigins);
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}