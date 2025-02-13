using System.ComponentModel.DataAnnotations;

namespace DealSpot.Models
{
	public class Product
	{
		public int ID { get; set; }
		[Required(ErrorMessage = "Product name cannot be null")]
		[StringLength(3, ErrorMessage = "Product name must be at least 2 characters long.")]
		public string Name { get; set; }
		[Range(0, double.MaxValue, ErrorMessage = "Price cannot be less than 0")]
		public decimal Price { get; set; }
	}
}
