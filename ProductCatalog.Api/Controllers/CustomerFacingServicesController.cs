using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.CQRS;
using ProductCatalog.Api.CQRS.Commands.CustomerFacingServices;
using ProductCatalog.Api.CQRS.Queries.CustomerFacingServices;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.Controllers;

/// <summary>
/// API controller exposing CRUD operations for customer‑facing services
/// (CFS).  These services represent offerings that can be attached
/// to products or consumed directly by customers.  Endpoints follow
/// REST conventions and use the CQRS dispatcher to execute
/// commands and queries.
/// </summary>
[ApiController]
[Route("api/cfs")]
public class CustomerFacingServicesController : ControllerBase
{
    private readonly IDispatcher _dispatcher;
    public CustomerFacingServicesController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    // POST api/cfs
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CustomerFacingService service)
    {
        var created = await _dispatcher.Send(new CreateCustomerFacingServiceCommand { Service = service }, HttpContext.RequestAborted);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // GET api/cfs
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _dispatcher.Query(new GetAllCustomerFacingServicesQuery(), HttpContext.RequestAborted));

    // GET api/cfs/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _dispatcher.Query(new GetCustomerFacingServiceByIdQuery { Id = id }, HttpContext.RequestAborted);
        return result is null ? NotFound() : Ok(result);
    }

    // PUT api/cfs/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] CustomerFacingService service)
    {
        service.Id = id;
        var updated = await _dispatcher.Send(new UpdateCustomerFacingServiceCommand { Service = service }, HttpContext.RequestAborted);
        return updated is null ? NotFound() : Ok(updated);
    }

    // DELETE api/cfs/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _dispatcher.Send(new DeleteCustomerFacingServiceCommand { Id = id }, HttpContext.RequestAborted);
        return ok ? Ok() : NotFound();
    }
}