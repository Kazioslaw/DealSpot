using DealSpot.Models;

namespace DealSpot.Services
{
	public class UserService : IUserService
	{
		private User _user;
		private readonly IPasswordHasher _passwordHasher;

		public UserService(IPasswordHasher passwordHasher)
		{
			_passwordHasher = passwordHasher;
			/* Tymczasowe rozwiązanie mające na celu przetestowanie systemu autentykacji i generacji tokenu jwt.
			   Powinno to być zamienione na system Identity lub własnoręcznie skonfigurowany system logowania i rejstrowania użytkowników,
			   oraz przechowywania ich w bazie danych */
			_user = new User()
			{
				Username = "Employee",
				PasswordHash = _passwordHasher.Hash("Employee@133")
			};
		}

		public User GetUser(string username)
		{
			if (_user.Username == username)
			{
				return _user;
			}
			return null;
		}
	}
}
