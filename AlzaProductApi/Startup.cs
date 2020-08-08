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
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;

namespace AlzaProductApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }


		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ProductContext>(options =>
															options.UseSqlServer(
																Configuration["ConnectionString"]));
			services.AddControllers();
			services.AddAutoMapper(typeof(Startup));
			services.AddSwaggerGen(opt =>
			{
				opt.SwaggerDoc("v1", new OpenApiInfo() { Title = "Alza product API", Version = "v1" });
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				opt.IncludeXmlComments(xmlPath);
			});
		}


		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper, ProductContext ctx, ILogger<Startup> logger)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			if (!env.IsDevelopment())
				app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Alza product API"));
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
								   logger.LogWarning(exception, "Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", exception.GetType().Name, exception.Message, retry, 3);
							   })
				 .Execute(() =>
				  {
					  if (context.Database.GetPendingMigrations().Any())
					  {
						  context.Database.Migrate();
					  }
				  });
		}
	}
}
