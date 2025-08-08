using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.CQRS;
using ProductCatalog.Api.CQRS.Commands.ProductOfferings;
using ProductCatalog.Api.CQRS.Queries.ProductOfferings;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.Controllers;

/// <summary>
/// API controller exposing CRUD operations for product offerings.  A
/// product offering aggregates products into bundles with pricing
/// information.  Endpoints follow REST conventions and use the
/// CQRS dispatcher.
/// </summary>
[ApiController]
[Route("api/offerings")]
public class OfferingsController : ControllerBase
{
    private readonly IDispatcher _dispatcher;
    public OfferingsController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    // POST api/offerings
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ProductOffering offering)
    {
        var created = await _dispatcher.Send(new CreateProductOfferingCommand { ProductOffering = offering }, HttpContext.RequestAborted);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // GET api/offerings
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _dispatcher.Query(new GetAllProductOfferingsQuery(), HttpContext.RequestAborted));

    // GET api/offerings/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _dispatcher.Query(new GetProductOfferingByIdQuery { Id = id }, HttpContext.RequestAborted);
        return result is null ? NotFound() : Ok(result);
    }

    // PUT api/offerings/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] ProductOffering offering)
    {
        offering.Id = id;
        var updated = await _dispatcher.Send(new UpdateProductOfferingCommand { ProductOffering = offering }, HttpContext.RequestAborted);
        return updated is null ? NotFound() : Ok(updated);
    }

    // DELETE api/offerings/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _dispatcher.Send(new DeleteProductOfferingCommand { Id = id }, HttpContext.RequestAborted);
        return ok ? Ok() : NotFound();
    }
}