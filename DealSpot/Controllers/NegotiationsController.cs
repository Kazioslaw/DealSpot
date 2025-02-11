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

		[HttpGet("{id:int}")]
		public IActionResult GetNegotation(int id)
		{
			var negotiation = _negotiationService.GetNegotiation(id);
			if (negotiation == null)
			{
				return NotFound("The negotiation with this id is not found");
			}
			return Ok(negotiation);
		}

		[HttpPost]
		public IActionResult StartNegotiation(int productID, decimal proposedPrice)
		{
			var product = _productService.GetProduct(productID);
			if (product == null)
			{
				return BadRequest();
			}
			var negotiation = new Negotiation()
			{
				ProductID = productID,
				Product = product,
				ActualPrice = product.Price,
				ProposedPrice = proposedPrice,
				NegotiationStartTime = DateTime.Now,
				NegotiationPriceProposedTime = DateTime.Now,
				AttemptCount = 1,
				Status = NegotiationStatus.PriceProposed,
			};
			_negotiationService.CreateNegotiation(negotiation);
			return CreatedAtAction(nameof(StartNegotiation), new { ID = negotiation.ID }, negotiation);
		}

		[HttpPost("{id:int}")]
		public IActionResult NegotiationNewPrice(int id, decimal proposedPrice)
		{
			var negotiation = _negotiationService.GetNegotiation(id);
			if (negotiation == null)
			{
				return BadRequest();
			}
			if (negotiation.AttemptCount > 2)
			{
				NegotiationCanceledPrice(id);
				return BadRequest("The number of price negotiation attempts has been exceeded");
			}
			if (DateTime.Now > negotiation.LastRejectedPriceTime.Value.AddDays(7))
			{
				NegotiationCanceledPrice(id);
				return BadRequest("The time limit for negotiation has passed");
			}

			negotiation.ProposedPrice = proposedPrice;
			negotiation.LastRejectedPriceTime = null;
			negotiation.NegotiationPriceProposedTime = DateTime.Now;
			negotiation.AttemptCount = negotiation.AttemptCount + 1;

			_negotiationService.UpdateNegotiation(negotiation);
			return Ok(negotiation);
		}

		[HttpPost("{id:int}/accept")]
		public IActionResult NegotiationAcceptPrice(int id)
		{
			var negotiation = _negotiationService.GetNegotiation(id);
			if (negotiation == null)
			{
				return BadRequest();
			}
			negotiation.Status = NegotiationStatus.PriceAccepted;
			_negotiationService.UpdateNegotiation(negotiation);
			return Ok("Price was accepted");

		}

		[HttpPost("{id:int}/reject")]
		public IActionResult NegotiationRejectedPrice(int id)
		{
			var negotiation = _negotiationService.GetNegotiation(id);
			if (negotiation == null)
			{
				return BadRequest();
			}
			negotiation.Status = NegotiationStatus.PriceRejected;
			negotiation.LastRejectedPriceTime = DateTime.Now;
			_negotiationService.UpdateNegotiation(negotiation);
			return Ok("Price was rejected");
		}

		[HttpPost("{id:int}/cancel")]
		public IActionResult NegotiationCanceledPrice(int id)
		{
			var negotiation = _negotiationService.GetNegotiation(id);
			if (negotiation == null)
			{
				return BadRequest();
			}
			negotiation.Status = NegotiationStatus.NegotiationCancelled;
			_negotiationService.UpdateNegotiation(negotiation);
			return Ok("Negotiation was cancelled");
		}
	}


}