using DealSpot.Models;
using Microsoft.EntityFrameworkCore;

namespace DealSpot.Data
{
	public class DealSpotDBContext : DbContext
	{
		public DealSpotDBContext(DbContextOptions<DealSpotDBContext> options) : base(options)
		{

		}

		public DbSet<Product> Product { get; set; }
		public DbSet<Negotiation> Negotiation { get; set; }


	}
}
