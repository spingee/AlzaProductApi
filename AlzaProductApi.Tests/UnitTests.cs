using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlzaProductApi.Business.Entities;
using AlzaProductApi.Controllers;
using AlzaProductApi.Data;
using AlzaProductApi.ViewModels;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlzaProductApi.Tests
{
	[TestClass]
	public class UnitTests
	{
		private IMapper _mapper;
		private string _databaseName;

		[ClassInitialize]
		public static void ClassInitialize(TestContext testContext)
		{
			//using var context = CreateIRealDbProductContext();
			//if (context.Database.GetPendingMigrations().Any())
			//{
			//	context.Database.Migrate();
			//}
		}

		[TestInitialize]
		public void Init()
		{
			var config = new MapperConfiguration(opts =>
												 {
													 opts.AddProfile(typeof(AutoMapping));
												 });
			_mapper = config.CreateMapper();
			_databaseName = Guid.NewGuid().ToString();
		}
		[DataRow(true, DisplayName = "InMemoryDb")]
		[DataRow(false, DisplayName = "RealDb")]
		[DataTestMethod]
		public async Task GetAll_ShouldReturnAllProducts(bool inMemory)
		{
			var dbProducts = new List<Product>
			{
				Product.Create(1, "Hardrive", 2000.89M, "http:\\temp.uri", "SSD MVE"),
				Product.Create(2, "Hardrive", 2000.89M, "http:\\temp.uri", "SSD MVE")
			};
			await using (var ctx = CreateProductContext(inMemory))
			{
				await ctx.Products.AddRangeAsync(dbProducts);
				await ctx.SaveChangesAsync();
			}

			var response = await CallProductController(controller => controller.Get());

			response.Should().BeOfType<OkObjectResult>();
			var ok = (OkObjectResult)response;
			ok.Value.Should().BeAssignableTo<IEnumerable<Models.Product>>();
			var result = (IEnumerable<Models.Product>)ok.Value;
			result.Should().BeEquivalentTo(dbProducts.Select(f => _mapper.Map<Models.Product>(f)));

		}

		[DataTestMethod]
		public async Task GetPaginated_ShouldCorrectPaginatedResult()
		{
			var dbProducts = new List<Product>
							 {
								 Product.Create(1, "Hardrive1", 2000.89M, "http:\\temp.uri", "SSD MVE"),
								 Product.Create(2, "Hardrive2", 3000.89M, "http:\\temp.uri", "SSD MVE"),
								 Product.Create(3, "Hardrive3", 2000.89M, "http:\\temp.uri", "SSD MVE"),
								 Product.Create(4, "Hardrive4", 3000.89M, "http:\\temp.uri", "SSD MVE"),
								 Product.Create(5, "Hardrive5", 2000.89M, "http:\\temp.uri", "SSD MVE"),
								 Product.Create(6, "Hardrive6", 3000.89M, "http:\\temp.uri", "SSD MVE"),
								 Product.Create(7, "Hardrive7", 2000.89M, "http:\\temp.uri", "SSD MVE"),
								 Product.Create(8, "Hardrive8", 3000.89M, "http:\\temp.uri", "SSD MVE")
							 };
			await using (var ctx = CreateInMemoryProductContext())
			{
				await ctx.Products.AddRangeAsync(dbProducts);
				await ctx.SaveChangesAsync();
			}

			var response = await CallProductController(controller => controller.GetPaginated(2, 2));

			response.Should().BeOfType<OkObjectResult>();
			var ok = (OkObjectResult)response;
			ok.Value.Should().BeAssignableTo<PaginatedItemsViewModel<Models.Product>>();
			var result = (PaginatedItemsViewModel<Models.Product>)ok.Value;
			result.Count.Should().Be(dbProducts.Count);
			result.Data.Should().BeEquivalentTo(dbProducts.Skip(2 * 2).Take(2).Select(f => _mapper.Map<Models.Product>(f)));

		}

		[DataTestMethod]
		public async Task GetPaginated_PageOutOfBounds_ShouldEmptyPaginatedResult()
		{
			var dbProducts = new List<Product>
							 {
								 Product.Create(1, "Hardrive1", 2000.89M, "http:\\temp.uri", "SSD MVE"),
								 Product.Create(2, "Hardrive2", 3000.89M, "http:\\temp.uri", "SSD MVE"),
								 Product.Create(3, "Hardrive3", 2000.89M, "http:\\temp.uri", "SSD MVE"),
								 Product.Create(4, "Hardrive4", 3000.89M, "http:\\temp.uri", "SSD MVE"),
								 Product.Create(5, "Hardrive5", 2000.89M, "http:\\temp.uri", "SSD MVE"),
								 Product.Create(6, "Hardrive6", 3000.89M, "http:\\temp.uri", "SSD MVE"),
								 Product.Create(7, "Hardrive7", 2000.89M, "http:\\temp.uri", "SSD MVE"),
								 Product.Create(8, "Hardrive8", 3000.89M, "http:\\temp.uri", "SSD MVE")
							 };
			await using (var ctx = CreateInMemoryProductContext())
			{
				await ctx.Products.AddRangeAsync(dbProducts);
				await ctx.SaveChangesAsync();
			}

			var response = await CallProductController(controller => controller.GetPaginated(20, 10));

			response.Should().BeOfType<OkObjectResult>();
			var ok = (OkObjectResult)response;
			ok.Value.Should().BeAssignableTo<PaginatedItemsViewModel<Models.Product>>();
			var result = (PaginatedItemsViewModel<Models.Product>)ok.Value;
			result.Count.Should().Be(dbProducts.Count);
			result.Data.Should().BeEmpty();

		}

		[DataTestMethod]
		public async Task GetById_ShouldReturnCorrectProduct()
		{
			Product expected = Product.Create(1, "Hardrive1", 2000.89M, "http:\\temp.uri", "SSD MVE");
			var dbProducts = new List<Product>
							 {
								 expected,
								 Product.Create(2, "Hardrive2", 3000.89M, "http:\\temp.uri", "SSD MVE")
							 };
			await using (var ctx = CreateInMemoryProductContext())
			{
				await ctx.Products.AddRangeAsync(dbProducts);
				await ctx.SaveChangesAsync();
			}

			var response = await CallProductController(controller => controller.Get(1));

			response.Should().BeOfType<OkObjectResult>();
			var ok = (OkObjectResult)response;
			ok.Value.Should().BeAssignableTo<Models.Product>();
			var result = (Models.Product)ok.Value;
			result.Should().BeEquivalentTo(_mapper.Map<Models.Product>(expected));

		}

		[DataTestMethod]
		public async Task GetById_NonExistingProduct_ShouldNotFound()
		{
			Product expected = Product.Create(1, "Hardrive1", 2000.89M, "http:\\temp.uri", "SSD MVE");
			var dbProducts = new List<Product>
							 {
								 expected,
								 Product.Create(2, "Hardrive2", 3000.89M, "http:\\temp.uri", "SSD MVE")
							 };
			await using (var ctx = CreateInMemoryProductContext())
			{
				await ctx.Products.AddRangeAsync(dbProducts);
				await ctx.SaveChangesAsync();
			}

			var response = await CallProductController(controller => controller.Get(3));

			response.Should().BeOfType<NotFoundResult>();
		}

		[DataTestMethod]
		public async Task ChangeDescription_ShouldSuccessfullyChangeDescription()
		{
			Product expected = Product.Create(1, "Hardrive1", 2000.89M, "http:\\temp.uri", "SSD MVE");
			var dbProducts = new List<Product>
							 {
								 expected,
								 Product.Create(2, "Hardrive2", 3000.89M, "http:\\temp.uri", "SSD MVE")
							 };
			await using (var ctx = CreateInMemoryProductContext())
			{
				await ctx.Products.AddRangeAsync(dbProducts);
				await ctx.SaveChangesAsync();
			}

			var response = await CallProductController(controller => controller.ChangeDescription(2, "new description"));

			response.Should().BeOfType<OkResult>();

			var product = await CreateInMemoryProductContext().Products.FindAsync(2);
			product.Description.Should().Be("new description");
		}

		private async Task<T> CallProductController<T>(Func<ProductController, Task<T>> func)
		{
			await using var ctx = CreateInMemoryProductContext();
			var controller = new ProductController(ctx, _mapper);
			return await func(controller);
		}

		private ProductContext CreateProductContext(bool inMemory)
		{
			//if (inMemory)
				return CreateInMemoryProductContext();
			//return CreateIRealDbProductContext();
		}

		private ProductContext CreateInMemoryProductContext()
		{
			var dbContextOptions = new DbContextOptionsBuilder<ProductContext>().UseInMemoryDatabase(databaseName: _databaseName).Options;
			return new ProductContext(dbContextOptions);
		}

		private static ProductContext CreateIRealDbProductContext()
		{
			var builder = new DbContextOptionsBuilder<ProductContext>();
			var dbContextOptions = builder.UseSqlServer("Server=localhost;Database=AlzaProductTests;Integrated Security = true;MultipleActiveResultSets=true")
										   .Options;
			return new ProductContext(dbContextOptions);
		}
	}
}
