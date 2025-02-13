using DealSpot.Models;

namespace DealSpot.Services
{
	public interface INegotiationService
	{
		IEnumerable<Negotiation> GetNegotiations();
		Negotiation? GetNegotiation(int id);
		Negotiation CreateNegotiation(Negotiation negotiation);
		Negotiation UpdateNegotiation(Negotiation negotiation);

	}
}
