﻿using AlzaProductApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AlzaProductApi.Data;
using AlzaProductApi.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AlzaProductApi.Controllers;

/// <summary>
/// Api for products
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
[ApiVersion("2")]
[ApiController]
public class ProductController : ControllerBase
{
	private readonly ProductContext _db;
	private readonly IMapper _mapper;

	public ProductController(
		ProductContext db,
		IMapper mapper
	)
	{
		_db = db;
		_mapper = mapper;
	}
	/// <summary>
	/// Gets all products
	/// </summary>
	/// <returns>All available products</returns>
	[ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
	[MapToApiVersion("1")]
	[HttpGet]
	public async Task<IActionResult> Get()
	{
		return Ok(await _db.Products.ProjectTo<Product>(_mapper.ConfigurationProvider).ToListAsync());
	}

	/// <summary>
	/// Gets products by page
	/// </summary>
	/// <param name="pageIndex">Index starting with 0</param>
	/// <param name="pageSize">Size of page</param>
	/// <returns></returns>
	[ProducesResponseType(typeof(PaginatedItemsViewModel<Product>), (int)HttpStatusCode.OK)]
	[MapToApiVersion("2")]
	[HttpGet]
	public async Task<IActionResult> GetPaginated(uint pageIndex = 0, byte pageSize = 10)
	{
		var count = await _db.Products.LongCountAsync();
		var products = await _db.Products
		                        .Skip((int)(pageSize * pageIndex)).Take(pageSize)
		                        .ProjectTo<Product>(_mapper.ConfigurationProvider)
		                        .ToListAsync();

		return Ok(new PaginatedItemsViewModel<Product>((int)pageIndex, pageSize, count, products));

	}

	/// <summary>
	/// Gets product by its unique id
	/// </summary>
	/// <param name="id">Id of product</param>
	/// <returns>Product</returns>
	[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
	[ProducesResponseType((int)HttpStatusCode.NotFound)]
	[HttpGet("{id}", Name = "Get")]
	public async Task<IActionResult> Get(int id)
	{
		var product = await _db.Products.SingleOrDefaultAsync(f => f.Id == id);
		if (product == null)
			return NotFound();
		return Ok(_mapper.Map<Product>(product));
	}

	/// <summary>
	/// Updates product description
	/// </summary>
	/// <param name="id">Id of product</param>
	/// <param name="description">New description</param>
	/// <returns>Product</returns>
	[HttpPut("{id}",Name = "ChangeDescription")]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	[ProducesResponseType((int)HttpStatusCode.NotFound)]
	[ProducesResponseType((int)HttpStatusCode.BadRequest)]
	public async Task<IActionResult> ChangeDescription(int id, ChangeDescriptionRequest description)
	{
		var product = await _db.Products.SingleOrDefaultAsync(f => f.Id == id);
		if (product == null)
			return NotFound();
		product.ChangeDescription(description.Description);
		await _db.SaveChangesAsync();
		return Ok();
	}
}

/// <summary>
/// Request to change description
/// </summary>
public class ChangeDescriptionRequest
{
	/// <summary>
	/// Request to change description
	/// </summary>
	public string Description { get; set; }
}