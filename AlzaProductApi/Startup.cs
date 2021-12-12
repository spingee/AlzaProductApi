using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AlzaProductApi.Data;
using AlzaProductApi.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AlzaProductApi;

public class Startup
{
	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	public IConfiguration Configuration { get; }


	public void ConfigureServices(IServiceCollection services)
	{
		services.AddMvc(c =>
			c.Conventions.Add(
				new ApiExplorerGroupPerVersionConvention())); // decorate Controllers to distinguish SwaggerDoc (v1, v2, etc.)


		services.AddDbContext<ProductContext>(options =>
			options.UseSqlServer(
				Configuration["ConnectionString"]));
		services.AddControllers();
		services.AddAutoMapper(typeof(Startup));
		services.AddApiVersioning();

		services.AddApiVersioning(o =>
		{
			o.AssumeDefaultVersionWhenUnspecified = true;
			o.DefaultApiVersion = new ApiVersion(2, 0);
		});

		services.AddSwaggerGen(opt =>
		{
			opt.SwaggerDoc("v1",
				new OpenApiInfo()
				{
					Title = "Alza product API V1", Version = "v1",
					Description = "Api for manipulating Alza product catalog"
				});
			opt.SwaggerDoc("v2",
				new OpenApiInfo()
				{
					Title = "Alza product API V2", Version = "v2",
					Description = "Api for manipulating Alza product catalog"
				});
			var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
			opt.IncludeXmlComments(xmlPath);
			opt.CustomSchemaIds(x => x.FullName);
			opt.EnableAnnotations();
			// Apply the filters
			opt.OperationFilter<RemoveVersionFromParameter>();
			opt.DocumentFilter<ReplaceVersionWithExactValueInPath>();
			// Ensure the routes are added to the right Swagger doc
			opt.DocInclusionPredicate((version, desc) =>
			{
				if (!desc.TryGetMethodInfo(out MethodInfo methodInfo))
					return false;

				var versions = methodInfo.DeclaringType
				                         .GetCustomAttributes(true)
				                         .OfType<ApiVersionAttribute>()
				                         .SelectMany(attr => attr.Versions);

				var maps = methodInfo
				           .GetCustomAttributes(true)
				           .OfType<MapToApiVersionAttribute>()
				           .SelectMany(attr => attr.Versions)
				           .ToArray();

				return versions.Any(v => $"v{v.ToString()}" == version)
				       && (!maps.Any() || maps.Any(v => $"v{v.ToString()}" == version));
			});
		});
	}


	public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper, ProductContext ctx,
		ILogger<Startup> logger)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}

		if (!env.IsDevelopment())
			app.UseHttpsRedirection();

		app.UseRouting();

		app.UseAuthorization();

		app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

		app.UseSwagger();
		app.UseSwaggerUI(c =>
		{
			c.SwaggerEndpoint("/swagger/v1/swagger.json", "Alza product API V1");
			c.SwaggerEndpoint("/swagger/v2/swagger.json", "Alza product API V2");
		});
		//app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v2/swagger.json", "Alza product API V2"));
		mapper.ConfigurationProvider.AssertConfigurationIsValid();
		ApplyMigrations(ctx, logger);
	}

	private void ApplyMigrations(ProductContext context, ILogger<Startup> logger)
	{
		Policy.Handle<SqlException>()
		      .WaitAndRetry(retryCount: 3,
			      sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
			      onRetry: (exception, timeSpan, retry, ctx) =>
			      {
				      logger.LogWarning(exception,
					      "Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}",
					      exception.GetType().Name, exception.Message, retry, 3);
			      })
		      .Execute(() =>
		      {
			      if (context.Database.GetPendingMigrations().Any())
			      {
				      context.Database.Migrate();
			      }
		      });
	}

	private class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
	{
		public void Apply(ControllerModel controller)
		{
			var controllerNamespace = controller.ControllerType.Namespace; // e.g. "Controllers.v1"
			var apiVersion = controllerNamespace?.Split('.').Last().ToLower();

			controller.ApiExplorer.GroupName = apiVersion;
		}
	}
}