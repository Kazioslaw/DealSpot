using DealSpot.Models;
using DealSpot.Services.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DealSpot.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly IAuthenticationService _authenticationService;
		private readonly IConfiguration _configuration;

		public AuthenticationController(IAuthenticationService authenticationService, IConfiguration configuration)
		{
			_authenticationService = authenticationService;
			_configuration = configuration;
		}


		[HttpPost("login")]
		public IActionResult Login([FromBody] UserDTO user)
		{
			var status = _authenticationService.Authenticate(user);
			if (!status)
			{
				return Unauthorized("Invalid username or password");
			}

			string token = GenerateJwtToken(user);

			return Ok(token);
		}

		private string GenerateJwtToken(UserDTO user)
		{
			var jwtTokenHandler = new JwtSecurityTokenHandler();

			var key = Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"]);

			var tokenDescriptor = new SecurityTokenDescriptor()
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(JwtRegisteredClaimNames.Name, user.Username),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
					new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),

				}),

				Expires = DateTime.Now.AddDays(7),
				Issuer = _configuration["JwtConfig:Issuer"],
				Audience = _configuration["JwtConfig:Audience"],
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
			};

			var token = jwtTokenHandler.CreateToken(tokenDescriptor);
			var jwtToken = jwtTokenHandler.WriteToken(token);
			return jwtToken;
		}
	}
}
