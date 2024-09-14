﻿using Dorm.BLL.Interfaces;
using Dorm.Domain.DTO;
using Dorm.Server.Contracts.Commands.TicketCommands;
using Dorm.Server.Contracts.Queries.TicketQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dorm.Server.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    //[Authorize]
    public class TicketController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{ticketId}")]
        public async Task<IActionResult> GetTicketById([FromRoute] int ticketId)
        {
            TicketDto? ticket = await _mediator.Send(new GetTicketByIdQuery(ticketId));
            if (ticket == null)
                return NotFound();

            return Ok(ticket);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTickets()
        {
            IEnumerable<TicketDto> tickets = await _mediator.Send(new GetAllTicketsQuery());
            return Ok(tickets);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketCommand createTicketCommand)
        {
            TicketDto ticket = await _mediator.Send(createTicketCommand);
            return Ok(ticket); //CreatedAtAction(nameof(GetTicketById), new { ticketId = ticket.Id }, ticket);
        }

        [HttpDelete("{ticketId}")]
        public async Task<IActionResult> DeleteTicket([FromRoute] int ticketId)
        {
            return await _mediator.Send(new DeleteTicketCommand(ticketId)) ? NoContent() : NotFound();
        }

        [HttpPut("{ticketId}")]
        public async Task<IActionResult> UpdateTicket([FromRoute] int ticketId, [FromBody] TicketDto ticketDto)
        {
            TicketDto updatedTicket = await _mediator.Send(new UpdateTicketCommand(ticketId, ticketDto));
            if (updatedTicket == null)
                return NotFound();

            return Ok(updatedTicket);
        }
    }
}
