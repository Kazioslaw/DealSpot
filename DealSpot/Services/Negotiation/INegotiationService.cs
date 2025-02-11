using DealSpot.Models;

namespace DealSpot.Services
{
	public interface INegotiationService
	{
		Negotiation? GetNegotiation(int id);
		Negotiation CreateNegotiation(Negotiation negotiation);
		Negotiation UpdateNegotiation(Negotiation negotiation);

	}
}
