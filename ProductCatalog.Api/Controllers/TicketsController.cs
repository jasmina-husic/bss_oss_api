using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.CQRS;
using ProductCatalog.Api.CQRS.Commands.Tickets;
using ProductCatalog.Api.CQRS.Queries.Tickets;
using ProductCatalog.Api.Models;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly IDispatcher _dispatcher;
    public TicketsController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TicketCreateDto dto)
    {
        var created = await _dispatcher.Send(new CreateTicketCommand
        {
            CustomerCrmId = dto.CustomerCrmId,
            TicketingId = dto.TicketingId,
            Title = dto.Title,
            Status = dto.Status,
            Priority = dto.Priority,
            Owner = dto.Owner
        }, HttpContext.RequestAborted);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
        => await GetOne(id, null);

    [HttpGet("external/{ticketingId}")]
    public async Task<IActionResult> GetByExternal(string ticketingId)
        => await GetOne(null, ticketingId);

    private async Task<IActionResult> GetOne(int? id, string? ext)
    {
        var query = new GetTicketByIdQuery { Id = id ?? 0, TicketingId = ext };
        var result = await _dispatcher.Query(query, HttpContext.RequestAborted);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Ticket ticket)
    {
        ticket.Id = id;
        var updated = await _dispatcher.Send(new UpdateTicketCommand
        { Ticket = ticket, TicketingId = ticket.TicketingId }, HttpContext.RequestAborted);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpPut("external/{ticketingId}")]
    public async Task<IActionResult> PutByExternal(string ticketingId, [FromBody] Ticket ticket)
    {
        var updated = await _dispatcher.Send(new UpdateTicketCommand
        { Ticket = ticket, TicketingId = ticketingId }, HttpContext.RequestAborted);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _dispatcher.Send(new DeleteTicketCommand { Id = id }, HttpContext.RequestAborted);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("external/{ticketingId}")]
    public async Task<IActionResult> DeleteByExternal(string ticketingId)
    {
        var ok = await _dispatcher.Send(new DeleteTicketCommand { TicketingId = ticketingId }, HttpContext.RequestAborted);
        return ok ? Ok() : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await _dispatcher.Query(new GetAllTicketsQuery(), HttpContext.RequestAborted));
}