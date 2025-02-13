using DealSpot.Enums;
using DealSpot.Models;
using DealSpot.Services;
using Microsoft.AspNetCore.Mvc;

namespace DealSpot.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NegotiationsController : ControllerBase
	{
		private readonly INegotiationService _negotiationService;
		private readonly IProductService _productService;
		public NegotiationsController(INegotiationService negotiationService, IProductService productService)
		{
			_negotiationService = negotiationService;
			_productService = productService;
		}

		/// <summary>
		///	Zwraca listę negocjacji.
		/// </summary>
		/// <returns>
		/// `200 OK` - Zwracaca listę aktualnie prowadzonych oraz zakończonych negocjacji
		/// `204 NoContent` - Brak negocjacji do zwrócenia
		/// </returns>
		[HttpGet]
		[EndpointDescription("Zwraca listę negocjacji")]
		[ProducesResponseType(typeof(ICollection<Negotiation>), 200)]
		[ProducesResponseType(204)]
		public IActionResult GetNegotiationList()
		{
			var negotiationList = _negotiationService.GetNegotiations();
			if (negotiationList == null)
			{
				return NoContent();
			}
			return Ok(negotiationList);
		}

		/// <summary>
		/// Zwraca szczegóły dotyczące zasobu o podanym identyfikatorze
		/// </summary>
		/// <param name="id">Unikalny identyfikator szukanego zasobu</param>
		/// <returns>
		/// `200 OK` - Zwraca szczegółowe informacje dotyczące zasobu o podanym identyfikatorze
		/// `404 Not Found` - Nie znaleziono zasobu o podanym identyfikatorze
		/// </returns>

		[HttpGet("{id:int}")]
		[EndpointDescription("Zwraca szczegóły negocjacji o podanym identyfikatorze")]
		[ProducesResponseType(typeof(Negotiation), 200)]
		[ProducesResponseType(404)]
		public IActionResult GetNegotation(int id)
		{
			var negotiation = _negotiationService.GetNegotiation(id);
			if (negotiation == null)
			{
				return NotFound("The negotiation with this id is not found");
			}
			return Ok(negotiation);
		}

		/// <summary>
		/// Tworzy nową negocjacje na podstawie proponowanej ceny
		/// </summary>
		/// <param name="productID">Unikalny identyfikator produktu którego dotyczy nowa cena</param>
		/// <param name="proposedPrice">Cena zaproponowana przez klienta</param>
		/// <returns>
		/// `201 Created` - Poprawnie utworzono nową negocjację na podstawie zaproponowanej ceny
		/// `404 Not Found` - Nie znaleziono zasobu (produktu) o podanym identyfikatorze
		/// </returns>

		[HttpPost]
		[EndpointDescription("Tworzy nową negocjacje na podstawie proponowanej ceny")]
		[ProducesResponseType(typeof(Negotiation), 201)]
		[ProducesResponseType(404)]
		public IActionResult StartNegotiation(int productID, [FromBody] decimal proposedPrice)
		{
			var product = _productService.GetProduct(productID);
			if (product == null)
			{
				return NotFound();
			}
			var negotiation = new Negotiation()
			{
				ProductID = productID,
				Product = product,
				ActualPrice = product.Price,
				ProposedPrice = proposedPrice,
				StartTime = DateTime.UtcNow,
				PriceProposedTime = DateTime.UtcNow,
				AttemptCount = 1,
				Status = NegotiationStatus.PriceProposed,

			};
			_negotiationService.CreateNegotiation(negotiation);
			return CreatedAtAction(nameof(StartNegotiation), new { ID = negotiation.ID }, negotiation);
		}

		/// <summary>
		/// Aktualizuje zasób na podstawie zaproponowanej ceny.
		/// </summary>
		/// <param name="id">Unikalny identyfikator</param>
		/// <param name="proposedPrice">Cena proponowana przez klienta</param>
		/// <returns>
		/// `200 OK` - Zasób o podanym identyfikatorze został zaktualizowany o podaną cenę
		/// `400 Bad Request` - Brak możliwości aktualizaci ceny. Przekroczono limity "czasowy" lub "ilościowy" lub nowa proponowana cena jest równa cenie proponowanej uprzednio
		///						oraz brak możliwości aktualizacji ceny dla anulowanej negocjacji
		/// `404 Not Found` - Nie znaleziono zasobu o podanym identyfikatorze
		/// </returns>

		[HttpPost("negotiate/{id:int}")]
		[EndpointDescription("Aktualizuje negocjacje, przesyłając nową propozycję ceny.")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public IActionResult NegotiationNewPrice(int id, [FromBody] decimal proposedPrice)
		{
			var negotiation = _negotiationService.GetNegotiation(id);
			if (negotiation == null)
			{
				return NotFound($"Negotiation with this id is not found.");
			}


			negotiation.ProposedPrice = proposedPrice;
			negotiation.PriceProposedTime = DateTime.Now;
			negotiation.AttemptCount = negotiation.AttemptCount + 1;

			if (negotiation.LastRejectedTime != null)
			{
				if (negotiation.PriceProposedTime > negotiation.LastRejectedTime.Value.AddDays(7))
				{
					NegotiationCanceled(id);
					return BadRequest("You can no longer negotiate the price as the allowed period has passed.");
				}
			}
			if (negotiation.AttemptCount >= 3)
			{
				NegotiationCanceled(id);
				return BadRequest("You have reached maximum try of price negotiation");
			}

			if (negotiation.Status == NegotiationStatus.NegotiationCancelled)
			{
				return BadRequest("You can't propose new price to cancelled negotiation");
			}

			if (negotiation.ProposedPrice == negotiation.LastRejectedPrice)
			{
				NegotiationRejectedPrice(id);
				return BadRequest("This price was rejected last time. Please propose a different price");
			}
			if (negotiation.Status == NegotiationStatus.NegotiationCancelled)
			{
				return BadRequest("This negotiation has already been cancelled.");
			}


			return Ok(new { Message = "The proposed price has ben successfully submitted", negotiation });
		}

		/// <summary>
		/// Cena jest akceptowana, status negocjacji zmieniany jest na 'PriceAccept'
		/// </summary>
		/// <param name="id">Unikalny identyfikator zasobu</param>
		/// <returns>
		/// `200 OK` - Poprawnie zaakceptowano cenę zmieniając status zasobu
		/// `404 Not Found` - Brak zasobu o podanym identyfikatorze
		/// </returns> 

		[HttpPost("{id:int}/accept")]
		[EndpointDescription("Akceptuje cenę zmieniając status negocjacji na 'PriceAccept'")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public IActionResult NegotiationAcceptPrice(int id)
		{
			var negotiation = _negotiationService.GetNegotiation(id);
			if (negotiation == null)
			{
				return NotFound("The negotiation with this id is not found");
			}
			negotiation.LastRejectedTime = null;
			negotiation.Status = NegotiationStatus.PriceAccepted;
			_negotiationService.UpdateNegotiation(negotiation);
			return Ok("The price has been accepted successfully and the negotiation is now closed.");

		}

		/// <summary>
		/// Cena jest odrzucona, status negocjacji zmieniany jest na 'PriceRejected'
		/// </summary>
		/// <param name="id">Unikalny identyfikator zasobu</param>
		/// <returns>
		/// `200 OK` - Poprawnie odrzucono cenę zmieniając status zasobu na 'PriceRejected'
		/// `404 Not Found` - Brak zasobu o podanym identyfikatorze 
		/// </returns>

		[HttpPost("{id:int}/reject")]
		[EndpointDescription("Odrzuca cenę zmieniając status negocjacji na 'Price rejected'.")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public IActionResult NegotiationRejectedPrice(int id)
		{
			var negotiation = _negotiationService.GetNegotiation(id);
			if (negotiation == null)
			{
				return NotFound("Negotiation with this id is not found.");
			}
			negotiation.LastRejectedPrice = negotiation.ProposedPrice;
			negotiation.Status = NegotiationStatus.PriceRejected;
			negotiation.LastRejectedTime = DateTime.Now;
			_negotiationService.UpdateNegotiation(negotiation);
			return Ok("The price has been rejected and a new negotiation attempt can be made.");
		}

		/// <summary>
		/// Negocjacja jest anulowana, status negocjacji zmieniany jest na 'NegotiationCancelled'
		/// </summary>
		/// <param name="id">Unikalny identyfikator zasobu</param>
		/// <returns>
		/// `200 OK` - Poprawnie anulowano negocjację zmieniając status zasobu na 'NegotiationCancelled'
		/// `404 Not Found` - Brak zasobu o podanym identyfikatorze
		/// </returns>

		[HttpPost("{id:int}/cancel")]
		[EndpointDescription("Anuluje określoną negocjację")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public IActionResult NegotiationCanceled(int id)
		{
			var negotiation = _negotiationService.GetNegotiation(id);
			if (negotiation == null)
			{
				return NotFound("Negotiation with this id is not found.");
			}
			negotiation.Status = NegotiationStatus.NegotiationCancelled;
			_negotiationService.UpdateNegotiation(negotiation);
			return Ok("The negotiation has been cancelled and can no longer be continued.");
		}
	}
}