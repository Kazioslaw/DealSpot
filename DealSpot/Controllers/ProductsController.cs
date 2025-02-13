using DealSpot.Models;
using DealSpot.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DealSpot.Controllers
{
	[Route("api/[controller]")]
	[Authorize]
	[ApiController]
	public class ProductsController : ControllerBase
	{

		private readonly IProductService _productService;

		public ProductsController(IProductService productService)
		{
			_productService = productService;
		}

		/// <summary>
		/// Zwraca listę produktów
		/// </summary>
		/// <returns>
		/// `200 OK` - Zwraca listę zasobów
		/// `204 No Content` - Brak zasobów do zwrócenia
		/// </returns>
		[AllowAnonymous]
		[HttpGet]
		[EndpointDescription("Zwraca listę produktów")]
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

		/// <summary>
		/// Zwraca szczegółówe informacje zasobu o podanym identyfikatorze
		/// </summary>
		/// <param name="id">Unikalny identyfikator zasobu</param>
		/// <returns>
		/// `200 OK` - Zwraca szczegółowe informacje dotyczące zasobu
		/// `404 Not Found` - Brak zasobu o podanym identyfikatorze
		/// </returns>
		[HttpGet("{id:int}")]
		[AllowAnonymous]
    [EndpointDescription("Zwraca szczegółowe informacje zasobu o podanym identyfikatorze")]
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

		/// <summary>
		/// Tworzy podany zasób w oparciu o przesłane dane
		/// </summary>
		/// <param name="product">Obiekt reprezentujący produkt do utworzenia</param>
		/// <returns>
		/// `201 Created` - Pomyślnie utworzono zasób
		/// `400 Bad Request` - Brak informacji w przesłanym obiekcie
		/// </returns>
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
