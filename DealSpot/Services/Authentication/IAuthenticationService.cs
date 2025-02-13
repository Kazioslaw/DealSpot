using DealSpot.Models;
using Microsoft.AspNetCore.Mvc;

namespace DealSpot.Services.Authentication
{
	public interface IAuthenticationService
	{
		public bool Authenticate([FromBody] UserDTO user);

	}
}
