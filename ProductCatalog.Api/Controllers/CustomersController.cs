using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.CQRS;
using ProductCatalog.Api.CQRS.Commands.Customers;
using ProductCatalog.Api.CQRS.Queries.Customers;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IDispatcher _dispatcher;
    public CustomersController(IDispatcher dispatcher) => _dispatcher = dispatcher;


    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Customer customer)
    {
        var created = await _dispatcher.Send(new CreateCustomerCommand { Customer = customer },
                                             HttpContext.RequestAborted);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _dispatcher.Query(new GetAllCustomersQuery(), HttpContext.RequestAborted));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _dispatcher.Query(new GetCustomerByIdQuery { Id = id },
                                             HttpContext.RequestAborted);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("external/{crmId}")]
    public async Task<IActionResult> GetByCrm(string crmId)
    {
        var result = await _dispatcher.Query(new GetCustomerByCrmIdQuery { CrmId = crmId },
                                             HttpContext.RequestAborted);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Customer customer)
    {
        customer.Id = id;
        var updated = await _dispatcher.Send(new UpdateCustomerCommand { Customer = customer },
                                             HttpContext.RequestAborted);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpPut("external/{crmId}")]
    public async Task<IActionResult> PutByCrm(string crmId, [FromBody] Customer customer)
    {
        var updated = await _dispatcher.Send(new UpdateCustomerByCrmIdCommand
        { CrmId = crmId, Customer = customer }, HttpContext.RequestAborted);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _dispatcher.Send(new DeleteCustomerCommand { Id = id },
                                        HttpContext.RequestAborted);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("external/{crmId}")]
    public async Task<IActionResult> DeleteByCrm(string crmId)
    {
        var ok = await _dispatcher.Send(new DeleteCustomerByCrmIdCommand { CrmId = crmId },
                                        HttpContext.RequestAborted);
        return ok ? Ok() : NotFound();
    }
}