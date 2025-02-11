using DealSpot.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DealSpot.Models
{
	public class Negotiation
	{
		public int ID { get; set; }
		public int ProductID { get; set; }
		public Product? Product { get; set; }
		public decimal ActualPrice { get; set; }
		public decimal? LastRejectedPrice { get; set; }
		[Range(0, double.MaxValue, ErrorMessage = "Proposed price cannot be less than 0")]
		public decimal ProposedPrice { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime PriceProposedTime { get; set; }
		public DateTime? LastRejectedTime { get; set; }
		public int AttemptCount { get; set; }
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public NegotiationStatus Status { get; set; }
	}
}
