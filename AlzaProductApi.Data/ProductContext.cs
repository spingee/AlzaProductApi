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
			builder.Entity<Product>().Property("_timestamp").IsRowVersion();

		}

		public DbSet<Product> Products { get; set; }

		private static void SeedData(ModelBuilder builder)
		{


			builder.Entity<Product>().HasData(
				Product.Create(1, "Hardrive", 2000.89M, "http:\\temp.uri", "SSD MVE"),
				Product.Create(2, "Hardrive", 2000.89M, "http:\\temp.uri", "SSD MVE"),
				Product.Create(3, "Hardrive", 2000.89M, "http:\\temp.uri", "SSD MVE"),
				Product.Create(4, "Hardrive", 2000.89M, "http:\\temp.uri", "SSD MVE")
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