
using DealSpot.Data;
using DealSpot.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace DealSpot
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddDbContext<DealSpotDBContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
			builder.Services.AddScoped<IProductService, ProductService>();
			builder.Services.AddScoped<INegotiationService, NegotiationService>();

			builder.Services.AddControllers();
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddOpenApi();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
				app.MapScalarApiReference();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
