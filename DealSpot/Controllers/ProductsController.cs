using DealSpot.Models;
using DealSpot.Services;
using Microsoft.AspNetCore.Mvc;

namespace DealSpot.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{

		private readonly IProductService _productService;

		public ProductsController(IProductService productService)
		{
			_productService = productService;
		}

		// GET-ALL
		[HttpGet]
		[ProducesResponseType(typeof(ICollection<Product>), 200)]
		[ProducesResponseType(204)]
		public IActionResult GetProductList()
		{
			var productList = _productService.GetProducts();
			if (productList == null)
			{
				return NoContent();
			}
			return Ok(productList);
		}

		// GET
		[HttpGet("{id:int}")]
		[ProducesResponseType(typeof(Product), 200)]
		[ProducesResponseType(404)]
		public IActionResult GetProduct(int id)
		{
			var product = _productService.GetProduct(id);
			if (product == null)
			{
				return NotFound();
			}
			return Ok(product);
		}

		// POST
		[HttpPost]
		[ProducesResponseType(typeof(Product), 201)]
		[ProducesResponseType(400)]
		public IActionResult CreateProduct([FromBody] Product product)
		{
			if (product == null)
			{
				return BadRequest("Product cannot be null.");
			}
			_productService.CreateProduct(product);

			return CreatedAtAction(nameof(CreateProduct), new { Id = product.ID }, product);
		}
	}
}
