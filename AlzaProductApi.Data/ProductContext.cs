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
			builder.Entity<Product>().Property(f => f.Price).IsRequired().HasColumnType("decimal(18,2)");
			builder.Entity<Product>().Property("_timestamp").HasColumnName("TimeStamp").IsRowVersion();

		}

		public DbSet<Product> Products { get; set; }

		private static void SeedData(ModelBuilder builder)
		{
			builder.Entity<Product>().HasData(
				        new 
				        {
					        Id = 1,
					        Name = "Sportline",
					        Price = 299000.9M,
					        ImgUri = "http:\\temp.uri",
					        Description = "SportlineDescription"
				        },
				        new 
				        {
					        Id = 2,
					        Name = "RS",
					        Price = 599999M,
					        ImgUri = "http:\\temp.uri",
					        Description =
						        "RsDescription"
				        },
				        new 
				        {
					        Id = 3,
					        Name = "Active",
					        Price = 180000M,
					        ImgUri = "http:\\temp.uri",
					        Description = "ActiveDescription"
				        },
				        new 
				        {
					        Id = 4,
					        Name = "Ambition",
					        Price = 423123M,
					        ImgUri = "http:\\temp.uri",
					        Description = "AmbitionDescription"
				        },

				        new 
				        {
					        Id = 5,
					        Price = 123000M,
					        Name = "ActiveFabia",
					        ImgUri = "http:\\temp.uri",
					        Description = "ActiveFabiaDescription"
				        },
				        new 
				        {
					        Id = 6,
					        Price = 333333M,
					        Name = "AmbitionFabia",
					        ImgUri = "http:\\temp.uri",
					        Description = "AmbitionFabiaDescription"
				        },
				        new 
				        {
					        Id = 7,
					        Price = 350000M,
					        Name = "AmbitionOctavia",
					        ImgUri = "http:\\temp.uri",
					        Description = "AmbitionOctaviaDescription"
				        },
				        new 
				        {
					        Id = 8,
					        Name = "StyleOctavia",
					        ImgUri = "http:\\temp.uri",
					        Price = 699000M,
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