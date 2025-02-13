using DealSpot.Models;

namespace DealSpot.Services
{
	public interface IProductService
	{
		IEnumerable<Product> GetProducts();
		Product? GetProduct(int id);
		Product CreateProduct(Product product);

	}
}
