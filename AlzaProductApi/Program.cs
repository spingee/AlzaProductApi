using AlzaProductApi;
using AlzaProductApi.Data;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

//builder.WebHost.ConfigureKestrel(options => { });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	ProductContext productContext = scope.ServiceProvider.GetRequiredService<ProductContext>();
	startup.Configure(
		app, app.Environment,
		app.Services.GetRequiredService<IMapper>(),
		productContext,
		app.Services.GetRequiredService<ILogger<Startup>>());
}


app.Run();