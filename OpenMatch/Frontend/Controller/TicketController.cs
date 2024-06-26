using Frontend.Models;
using Frontend.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static Frontend.Models.TicketModels;

namespace Frontend.Controller
{
    [ApiController]
    [Route("v1/tickets")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OpenMatchTicketResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketPayload createTicketPayload, 
            CancellationToken cancellationToken)
        {
            var response = await _ticketService.CreateTicket(createTicketPayload, cancellationToken);
            return Ok(response);
        }

        [HttpGet("{ticketId}")]
        [ProducesResponseType(typeof(OpenMatchTicketResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTicket([FromRoute] string ticketId, CancellationToken cancellationToken)
        {
            var response = await _ticketService.GetTicket(ticketId, cancellationToken);
            return Ok(response);
        }

        [HttpDelete("{ticketId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTocket([FromRoute] string ticketId, CancellationToken cancellationToken)
        {
            var response = await _ticketService.DeleteTicket(ticketId, cancellationToken);
            return Ok(response);
        } 
    }
}
