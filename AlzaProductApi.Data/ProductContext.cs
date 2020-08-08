using System;
using AlzaProductApi.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AlzaProductApi.Data
{
	public class ProductContext : DbContext
	{
		public ProductContext(DbContextOptions<ProductContext> options)
			: base(options)
		{

		}

        protected override void OnModelCreating(ModelBuilder builder)
		{
			ConfigureEntities(builder);
			SeedData(builder);
            base.OnModelCreating(builder);
		}

		private static void ConfigureEntities(ModelBuilder builder)
		{
			builder.Entity<Product>().Property(f => f.Name).IsRequired().HasMaxLength(200);
			builder.Entity<Product>().Property(f => f.ImgUri).IsRequired();
			builder.Entity<Product>().Property(f => f.Description).IsRequired();

		}

		public DbSet<Product> Products { get; set; }

        private static void SeedData(ModelBuilder builder)
        {
            

            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Sportline",
                    Price = 299000.9M,
                    ImgUri = "http:\\temp.uri",
                    Description = "SportlineDescription"
                },
                new Product
                {
                    Id = 2,
                    Name = "RS",
                    Price = 599999,
                    ImgUri = "http:\\temp.uri",
                    Description =
                        "RsDescription"
                },
                new Product
                {
                    Id = 3,
                    Name = "Active",
                    Price = 180000,
                    ImgUri = "http:\\temp.uri",
                    Description = "ActiveDescription"
                },
                new Product
                {
                    Id = 4,
                    Name = "Ambition",
                    Price = 423123,
                    ImgUri = "http:\\temp.uri",
                    Description = "AmbitionDescription"
                },

                new Product
                {
                    Id = 5,
                    Price = 123000,
                    Name = "ActiveFabia",
                    ImgUri = "http:\\temp.uri",
                    Description = "ActiveFabiaDescription"
                },
                new Product
                {
                    Id = 6,
                    Price = 333333,
                    Name = "AmbitionFabia",
                    ImgUri = "http:\\temp.uri",
                    Description = "AmbitionFabiaDescription"
                },
                new Product
                {
                    Id = 7,
                    Price = 350000,
                    Name = "AmbitionOctavia",
                    ImgUri = "http:\\temp.uri",
                    Description = "AmbitionOctaviaDescription"
                },
                new Product
                {
                    Id = 8,
                    Name = "StyleOctavia",
                    ImgUri = "http:\\temp.uri",
                    Price = 699000,
                    Description = "StyleOctaviaDescription"
                }

            );
        }
    }

	public class ProductContextFactory : IDesignTimeDbContextFactory<ProductContext>
	{
		public ProductContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<ProductContext>();
			optionsBuilder.UseSqlServer("Server=localhost;Database=AlzaProduct;Integrated Security = true;MultipleActiveResultSets=true");

			return new ProductContext(optionsBuilder.Options);
		}
	}
}