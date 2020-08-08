using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlzaProductApi.Models;
using AutoMapper;

namespace AlzaProductApi
{
	public class AutoMapping : Profile
	{
		public AutoMapping()
		{
			CreateMap<Business.Entities.Product, Product>()
				;
			
		}
	}
}
