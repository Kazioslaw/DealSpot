
using DealSpot.Configurations;
using DealSpot.Data;
using DealSpot.Services;
using DealSpot.Services.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

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
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
			builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
			builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
			builder.Services.AddControllers();
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(jwt =>
				{
					jwt.SaveToken = true;
					jwt.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
						ValidAudience = builder.Configuration["JwtConfig:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Secret"])),
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						RequireExpirationTime = true
					};
				});



			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddOpenApi();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddAuthorization();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
				app.MapScalarApiReference();

			}

			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
