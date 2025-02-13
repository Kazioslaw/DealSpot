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

		[HttpGet]
		public IActionResult GetNegotiationList()
		{
			var negotiationList = _negotiationService.GetNegotiations();
			return Ok(negotiationList);
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
		public IActionResult StartNegotiation(int productID, [FromBody] decimal proposedPrice)
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
				StartTime = DateTime.UtcNow,
				PriceProposedTime = DateTime.UtcNow,
				AttemptCount = 1,
				Status = NegotiationStatus.PriceProposed,

			};
			_negotiationService.CreateNegotiation(negotiation);
			return CreatedAtAction(nameof(StartNegotiation), new { ID = negotiation.ID }, negotiation);
		}

		[HttpPost("negotiate/{id:int}")]
		public IActionResult NegotiationNewPrice(int id, [FromBody] decimal proposedPrice)
		{
			var negotiation = _negotiationService.GetNegotiation(id);
			if (negotiation == null)
			{
				return NotFound($"Negotiation with this id:{id} is not found.");
			}


			negotiation.ProposedPrice = proposedPrice;
			negotiation.PriceProposedTime = DateTime.Now;
			negotiation.AttemptCount = negotiation.AttemptCount + 1;

			if (negotiation.LastRejectedTime != null)
			{
				if (negotiation.PriceProposedTime > negotiation.LastRejectedTime.Value.AddDays(7))
				{
					NegotiationCanceledPrice(id);
					return BadRequest("You can no longer negotiate the price as the allowed period has passed.");
				}
			}
			if (negotiation.AttemptCount >= 3)
			{
				NegotiationCanceledPrice(id);
				return BadRequest("You have reached maximum try of price negotiation");
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

		[HttpPost("{id:int}/accept")]
		public IActionResult NegotiationAcceptPrice(int id)
		{
			var negotiation = _negotiationService.GetNegotiation(id);
			if (negotiation == null)
			{
				return BadRequest();
			}
			negotiation.LastRejectedTime = null;
			negotiation.Status = NegotiationStatus.PriceAccepted;
			_negotiationService.UpdateNegotiation(negotiation);
			return Ok("The price has been accepted successfully and the negotiation is now closed.");

		}

		[HttpPost("{id:int}/reject")]
		public IActionResult NegotiationRejectedPrice(int id)
		{
			var negotiation = _negotiationService.GetNegotiation(id);
			if (negotiation == null)
			{
				return NotFound($"Negotiation with this id:{id} is not found.");
			}
			negotiation.LastRejectedPrice = negotiation.ProposedPrice;
			negotiation.Status = NegotiationStatus.PriceRejected;
			negotiation.LastRejectedTime = DateTime.Now;
			_negotiationService.UpdateNegotiation(negotiation);
			return Ok("The price has been rejected and a new negotiation attempt can be made.");
		}

		[HttpPost("{id:int}/cancel")]
		public IActionResult NegotiationCanceledPrice(int id)
		{
			var negotiation = _negotiationService.GetNegotiation(id);
			if (negotiation == null)
			{
				return NotFound($"Negotiation with this id:{id} is not found.");
			}
			negotiation.Status = NegotiationStatus.NegotiationCancelled;
			_negotiationService.UpdateNegotiation(negotiation);
			return Ok("The negotiation has been cancelled and can no longer be continued.");
		}
	}
}