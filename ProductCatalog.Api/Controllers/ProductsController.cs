using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.CQRS;
using ProductCatalog.Api.CQRS.Commands.Products;
using ProductCatalog.Api.CQRS.Queries.Products;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.Controllers;

/// <summary>
/// API controller exposing CRUD operations for products.  Endpoints
/// follow REST conventions and use the CQRS dispatcher to execute
/// commands and queries.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IDispatcher _dispatcher;
    public ProductsController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    // POST api/products
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Product product)
    {
        var created = await _dispatcher.Send(new CreateProductCommand { Product = product }, HttpContext.RequestAborted);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // GET api/products
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _dispatcher.Query(new GetAllProductsQuery(), HttpContext.RequestAborted));

    // GET api/products/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _dispatcher.Query(new GetProductByIdQuery { Id = id }, HttpContext.RequestAborted);
        return result is null ? NotFound() : Ok(result);
    }

    // PUT api/products/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Product product)
    {
        product.Id = id;
        var updated = await _dispatcher.Send(new UpdateProductCommand { Product = product }, HttpContext.RequestAborted);
        return updated is null ? NotFound() : Ok(updated);
    }

    // DELETE api/products/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _dispatcher.Send(new DeleteProductCommand { Id = id }, HttpContext.RequestAborted);
        return ok ? Ok() : NotFound();
    }
}