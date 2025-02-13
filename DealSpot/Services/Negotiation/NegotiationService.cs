using DealSpot.Data;
using DealSpot.Models;

namespace DealSpot.Services
{
	public class NegotiationService : INegotiationService
	{
		private readonly DealSpotDBContext _context;
		public NegotiationService(DealSpotDBContext context)
		{
			_context = context;
		}

		public Negotiation CreateNegotiation(Negotiation negotiation)
		{
			_context.Negotiation.Add(negotiation);
			_context.SaveChanges();
			return negotiation;
		}

		public Negotiation? GetNegotiation(int id)
		{
			return _context.Negotiation.Where(n => n.ID == id).FirstOrDefault();
		}

		public Negotiation UpdateNegotiation(Negotiation negotiation)
		{
			_context.Negotiation.Update(negotiation);
			_context.SaveChanges();
			return negotiation;
		}
	}
}
