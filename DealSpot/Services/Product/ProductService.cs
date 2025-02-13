using DealSpot.Data;
using DealSpot.Models;

namespace DealSpot.Services
{
	public class ProductService : IProductService
	{
		private readonly DealSpotDBContext _context;
		public ProductService(DealSpotDBContext context)
		{
			_context = context;
		}

		// Put
		public Product CreateProduct(Product product)
		{
			_context.Product.Add(product);
			_context.SaveChanges();
			return product;
		}

		// Get 
		public Product? GetProduct(int id)
		{
			return _context.Product.Where(p => p.ID == id).FirstOrDefault();
		}

		// GetAll
		public IEnumerable<Product> GetProducts()
		{
			return _context.Product.ToList();
		}
	}
}
