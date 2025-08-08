using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.CQRS;
using ProductCatalog.Api.CQRS.Commands.Tickets;
using ProductCatalog.Api.CQRS.Commands.TicketComments;
using ProductCatalog.Api.CQRS.Queries.Tickets;
using ProductCatalog.Api.CQRS.Queries.TicketComments;
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
            DsId = dto.DsId ?? string.Empty,
            CustomerCrmId = dto.CustomerCrmId ?? string.Empty,
            RequesterId = dto.RequesterId,
            Subject = dto.Subject,
            Description = dto.Description,
            Status = dto.Status,
            Priority = dto.Priority,
            Submitter = dto.Submitter,
            Assignee = dto.Assignee
        }, HttpContext.RequestAborted);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
        => await GetOne(id, null);

    [HttpGet("external/{dsId}")]
    public async Task<IActionResult> GetByExternal(string dsId)
        => await GetOne(null, dsId);

    private async Task<IActionResult> GetOne(int? id, string? dsId)
    {
        var query = new GetTicketByIdQuery { Id = id ?? 0, DsId = dsId };
        var result = await _dispatcher.Query(query, HttpContext.RequestAborted);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Ticket ticket)
    {
        // Use UpdateTicketCommand and map fields
        var updated = await _dispatcher.Send(new UpdateTicketCommand
        {
            Id = id,
            DsId = ticket.DsId,
            RequesterId = ticket.RequesterId,
            Subject = ticket.Subject,
            Description = ticket.Description,
            Status = ticket.Status,
            Priority = ticket.Priority,
            Submitter = ticket.Submitter,
            Assignee = ticket.Assignee
        }, HttpContext.RequestAborted);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpPut("external/{dsId}")]
    public async Task<IActionResult> PutByExternal(string dsId, [FromBody] Ticket ticket)
    {
        var updated = await _dispatcher.Send(new UpdateTicketCommand
        {
            DsId = dsId,
            RequesterId = ticket.RequesterId,
            Subject = ticket.Subject,
            Description = ticket.Description,
            Status = ticket.Status,
            Priority = ticket.Priority,
            Submitter = ticket.Submitter,
            Assignee = ticket.Assignee
        }, HttpContext.RequestAborted);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _dispatcher.Send(new DeleteTicketCommand { Id = id }, HttpContext.RequestAborted);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("external/{dsId}")]
    public async Task<IActionResult> DeleteByExternal(string dsId)
    {
        var ok = await _dispatcher.Send(new DeleteTicketCommand { DsId = dsId }, HttpContext.RequestAborted);
        return ok ? Ok() : NotFound();
    }

    // Comments endpoints
    /// <summary>
    /// Create a new comment for a ticket identified by its internal ID.
    /// </summary>
    [HttpPost("{id:int}/comments")]
    public async Task<IActionResult> PostComment(int id, [FromBody] TicketComment comment)
    {
        var created = await _dispatcher.Send(new CreateTicketCommentCommand
        {
            TicketId = id,
            DsId = comment.DsId,
            Comment = comment.Comment,
            Author = comment.Author
        }, HttpContext.RequestAborted);
        return CreatedAtAction(nameof(GetComments), new { id = id }, created);
    }

    /// <summary>
    /// Create a new comment for a ticket identified by its DS ID.
    /// </summary>
    [HttpPost("external/{dsId}/comments")]
    public async Task<IActionResult> PostCommentByExternal(string dsId, [FromBody] TicketComment comment)
    {
        var created = await _dispatcher.Send(new CreateTicketCommentCommand
        {
            TicketDsId = dsId,
            DsId = comment.DsId,
            Comment = comment.Comment,
            Author = comment.Author
        }, HttpContext.RequestAborted);
        return CreatedAtAction(nameof(GetCommentsByExternal), new { dsId = dsId }, created);
    }

    /// <summary>
    /// Get all comments for a ticket by its internal ID.
    /// </summary>
    [HttpGet("{id:int}/comments")]
    public async Task<IActionResult> GetComments(int id)
    {
        var comments = await _dispatcher.Query(new GetCommentsByTicketQuery
        {
            TicketId = id
        }, HttpContext.RequestAborted);
        return Ok(comments);
    }

    /// <summary>
    /// Get all comments for a ticket by its DS ID.
    /// </summary>
    [HttpGet("external/{dsId}/comments")]
    public async Task<IActionResult> GetCommentsByExternal(string dsId)
    {
        var comments = await _dispatcher.Query(new GetCommentsByTicketQuery
        {
            TicketDsId = dsId
        }, HttpContext.RequestAborted);
        return Ok(comments);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await _dispatcher.Query(new GetAllTicketsQuery(), HttpContext.RequestAborted));
}