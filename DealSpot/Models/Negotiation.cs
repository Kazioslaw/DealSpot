using DealSpot.Enums;

namespace DealSpot.Models
{
	public class Negotiation
	{
		public int ID { get; set; }
		public int ProductID { get; set; }
		public Product Product { get; set; }
		public decimal ActualPrice { get; set; }
		public decimal ProposedPrice { get; set; }
		public DateTime NegotiationStartTime { get; set; }
		public DateTime LastRejectedPriceTime { get; set; }
		public int AttemptCount { get; set; }
		public NegotiationStatus Status { get; set; }
	}
}
