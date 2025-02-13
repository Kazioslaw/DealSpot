using DealSpot.Models;

namespace DealSpot.Services.Authentication
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly IUserService _userService;
		private readonly IPasswordHasher _passwordHasher;
		public AuthenticationService(IUserService userService, IPasswordHasher passwordHasher)
		{
			_userService = userService;
			_passwordHasher = passwordHasher;
		}
		public bool Authenticate(UserDTO user)
		{
			var existingUser = _userService.GetUser(user.Username);
			if (existingUser == null)
			{
				return false;
			}
			var isValidPassword = _passwordHasher.Verify(user.Password, existingUser.PasswordHash);
			if (!isValidPassword)
			{
				return false;
			}
			return true;
		}
	}
}
