using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AlzaProductApi.Infrastructure
{
	public class ReplaceVersionWithExactValueInPath : IDocumentFilter
	{
		public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
		{
			var paths = new OpenApiPaths();
			foreach (var path in swaggerDoc.Paths)
			{
				paths.Add(path.Key.Replace("v{version}", swaggerDoc.Info.Version), path.Value);
			}
			swaggerDoc.Paths = paths;
		}
    }

}
