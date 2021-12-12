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
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlzaProductApi.Tests;

[TestClass]
public class ProductApiTests
{
	private IMapper _mapper;
	private string _databaseName;
	private static bool _inMemory = true;
	private static string _connectionString;

	[ClassInitialize]
	public static void ClassInitialize(TestContext testContext)
	{
		var configurationRoot = GetIConfigurationRoot(testContext.DeploymentDirectory);
		_inMemory = configurationRoot.GetValue("UseInMemoryProvider", true);
		_connectionString = configurationRoot.GetValue("ConnectionString", "Server=localhost;Database=AlzaProductTests;Integrated Security = true;MultipleActiveResultSets=true");

		if (!_inMemory)
		{
			using var context = CreateRealDbProductContext();
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();
		}
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
		if (!_inMemory)
		{
			using var context = CreateRealDbProductContext();
			context.Products.RemoveRange(context.Products);
			context.SaveChanges();
		}
	}

	[TestMethod]
	public async Task GetAll_ShouldReturnAllProducts()
	{
		var dbProducts = new List<Product>
		{
			Product.Create( "Hardrive", 2000.89M, "http:\\temp.uri", "SSD MVE"),
			Product.Create( "Hardrive", 2000.89M, "http:\\temp.uri", "SSD MVE")
		};
		await using (var ctx = CreateProductContext())
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

	[TestMethod]
	public async Task GetPaginated_ShouldCorrectPaginatedResult()
	{
		var dbProducts = new List<Product>
		{
			Product.Create( "Hardrive1", 2000.89M, "http:\\temp.uri", "SSD MVE"),
			Product.Create( "Hardrive2", 3000.89M, "http:\\temp.uri", "SSD MVE"),
			Product.Create( "Hardrive3", 2000.89M, "http:\\temp.uri", "SSD MVE"),
			Product.Create( "Hardrive4", 3000.89M, "http:\\temp.uri", "SSD MVE"),
			Product.Create( "Hardrive5", 2000.89M, "http:\\temp.uri", "SSD MVE"),
			Product.Create( "Hardrive6", 3000.89M, "http:\\temp.uri", "SSD MVE"),
			Product.Create( "Hardrive7", 2000.89M, "http:\\temp.uri", "SSD MVE"),
			Product.Create( "Hardrive8", 3000.89M, "http:\\temp.uri", "SSD MVE")
		};
		await using (var ctx = CreateProductContext())
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

	[TestMethod]
	public async Task GetPaginated_PageOutOfBounds_ShouldEmptyPaginatedResult()
	{
		var dbProducts = new List<Product>
		{
			Product.Create( "Hardrive1", 2000.89M, "http:\\temp.uri", "SSD MVE"),
			Product.Create( "Hardrive2", 3000.89M, "http:\\temp.uri", "SSD MVE"),
			Product.Create( "Hardrive3", 2000.89M, "http:\\temp.uri", "SSD MVE"),
			Product.Create( "Hardrive4", 3000.89M, "http:\\temp.uri", "SSD MVE"),
			Product.Create( "Hardrive5", 2000.89M, "http:\\temp.uri", "SSD MVE"),
			Product.Create( "Hardrive6", 3000.89M, "http:\\temp.uri", "SSD MVE"),
			Product.Create( "Hardrive7", 2000.89M, "http:\\temp.uri", "SSD MVE"),
			Product.Create( "Hardrive8", 3000.89M, "http:\\temp.uri", "SSD MVE")
		};
		await using var ctx = CreateProductContext();
		await ctx.Products.AddRangeAsync(dbProducts);
		await ctx.SaveChangesAsync();

		var response = await CallProductController(controller => controller.GetPaginated(20, 10));

		response.Should().BeOfType<OkObjectResult>();
		var ok = (OkObjectResult)response;
		ok.Value.Should().BeAssignableTo<PaginatedItemsViewModel<Models.Product>>();
		var result = (PaginatedItemsViewModel<Models.Product>)ok.Value;
		result.Count.Should().Be(dbProducts.Count);
		result.Data.Should().BeEmpty();

	}

	[TestMethod]
	public async Task GetById_ShouldReturnCorrectProduct()
	{
		Product expected = Product.Create("Hardrive1", 2000.89M, "http:\\temp.uri", "SSD MVE");
		var dbProducts = new List<Product>
		{
			expected,
			Product.Create( "Hardrive2", 3000.89M, "http:\\temp.uri", "SSD MVE")
		};
		await using var ctx = CreateProductContext();
		await ctx.Products.AddRangeAsync(dbProducts);
		await ctx.SaveChangesAsync();


		var response = await CallProductController(controller => controller.Get(expected.Id));

		response.Should().BeOfType<OkObjectResult>();
		var ok = (OkObjectResult)response;
		ok.Value.Should().BeAssignableTo<Models.Product>();
		var result = (Models.Product)ok.Value;
		result.Should().BeEquivalentTo(_mapper.Map<Models.Product>(expected));

	}

	[TestMethod]
	public async Task GetById_NonExistingProduct_ShouldNotFound()
	{
		Product expected = Product.Create("Hardrive1", 2000.89M, "http:\\temp.uri", "SSD MVE");
		var dbProducts = new List<Product>
		{
			expected,
			Product.Create("Hardrive2", 3000.89M, "http:\\temp.uri", "SSD MVE")
		};
		await using var ctx = CreateProductContext();
		await ctx.Products.AddRangeAsync(dbProducts);
		await ctx.SaveChangesAsync();

		var response = await CallProductController(controller => controller.Get(dbProducts.Max(f => f.Id) + 1));

		response.Should().BeOfType<NotFoundResult>();
	}

	[TestMethod]
	public async Task ChangeDescription_ShouldSuccessfullyChangeDescription()
	{
		Product expected = Product.Create("Hardrive1", 2000.89M, "http:\\temp.uri", "SSD MVE");
		var dbProducts = new List<Product>
		{
			expected,
			Product.Create("Hardrive2", 3000.89M, "http:\\temp.uri", "SSD MVE")
		};
		await using var ctx = CreateProductContext();
		await ctx.Products.AddRangeAsync(dbProducts);
		await ctx.SaveChangesAsync();

		var newDescription = "new description";
		var response =
			await CallProductController(controller => controller.ChangeDescription(expected.Id, new ChangeDescriptionRequest { Description = newDescription }));
		response.Should().BeOfType<OkResult>();
		await using var productContext = CreateProductContext();
		var product = await productContext.Products.FindAsync(expected.Id);
		product.Should().NotBeNull();
		product.Description.Should().Be(newDescription);
	}

	private async Task<T> CallProductController<T>(Func<ProductController, Task<T>> func)
	{
		await using var ctx = CreateProductContext();
		var controller = new ProductController(ctx, _mapper);
		return await func(controller);
	}

	private ProductContext CreateProductContext()
	{
		if (_inMemory)
			return CreateInMemoryProductContext();
		return CreateRealDbProductContext();
	}

	private ProductContext CreateInMemoryProductContext()
	{
		var dbContextOptions = new DbContextOptionsBuilder<ProductContext>().UseInMemoryDatabase(databaseName: _databaseName).Options;
		return new ProductContext(dbContextOptions);
	}

	private static ProductContext CreateRealDbProductContext()
	{
		var builder = new DbContextOptionsBuilder<ProductContext>();
		var dbContextOptions = builder.UseSqlServer(_connectionString)
		                              .Options;
		return new ProductContext(dbContextOptions);
	}

	public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
	{
		return new ConfigurationBuilder()
		       .SetBasePath(outputPath)
		       .AddJsonFile("appsettings.json", optional: true)
		       //.AddUserSecrets("e3dfcccf-0cb3-423a-b302-e3e92e95c128")
		       .AddEnvironmentVariables("TEST_ALZA_")
		       .Build();
	}
}