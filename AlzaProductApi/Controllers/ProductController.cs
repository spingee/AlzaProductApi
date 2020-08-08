using AlzaProductApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using AlzaProductApi.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace AlzaProductApi.Controllers
{
	[Route("api/[controller]")]
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
		/// <returns></returns>
		[HttpGet]
		public IEnumerable<Product> Get()
		{
			return _db.Products.ProjectTo<Product>(_mapper.ConfigurationProvider).ToList();
		}

		/// <summary>
		/// Gets product by its unique id
		/// </summary>
		/// <param name="id">Id of product</param>
		/// <returns>Product</returns>
		[HttpGet("{id}", Name = "Get")]
		public ActionResult<Product> Get(int id)
		{
			var product = _db.Products.SingleOrDefault(f => f.Id == id);
			if (product == null)
				return NotFound();
			return _mapper.Map<Product>(product);
		}
	}
}
