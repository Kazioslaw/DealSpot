using DealSpot.Models;

namespace DealSpot.Services
{
	public interface IUserService
	{
		public User GetUser(string username);
	}
}
