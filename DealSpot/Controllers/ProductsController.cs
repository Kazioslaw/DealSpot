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
		public IActionResult GetProductList()
		{
			var products = _productService.GetProducts();
			return Ok(products);
		}

		// GET
		[HttpGet("{id:int}")]
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
		public IActionResult CreateProduct([FromBody] Product product)
		{
			if (product == null)
			{
				return BadRequest();
			}
			_productService.CreateProduct(product);

			return CreatedAtAction(nameof(CreateProduct), new { Id = product.ID }, product);
		}
	}
}
