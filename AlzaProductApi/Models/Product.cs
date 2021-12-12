using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlzaProductApi.Models;

/// <summary>
/// Represents product from Alza catalog
/// </summary>
public class Product
{
	/// <summary>
	/// Unique identification of product
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	/// Name of product
	/// </summary>
	public string Name { get; set; }
	/// <summary>
	/// Uri to product image
	/// </summary>
	public string ImgUri { get; set; }
	/// <summary>
	/// Price of product
	/// </summary>
	public decimal Price { get; set; }
	/// <summary>
	/// Detailed description of product
	/// </summary>
	public string Description { get; set; }
}