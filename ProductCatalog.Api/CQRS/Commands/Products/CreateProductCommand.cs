using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Commands.Products;

/// <summary>
/// Command to create a new product.  The supplied <see cref="Product"/>
/// instance will be persisted and returned with its generated Id.
/// </summary>
public class CreateProductCommand : ICommand<Product>
{
    public Product Product { get; set; } = null!;
}

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Product>
{
    private readonly ProductCatalogDbContext _ctx;
    public CreateProductCommandHandler(ProductCatalogDbContext ctx) => _ctx = ctx;
    public async Task<Product> Handle(CreateProductCommand cmd, CancellationToken ct)
    {
        // clear the Id if supplied so that EF generates a new one
        if (cmd.Product.Id != 0)
            cmd.Product.Id = 0;
        _ctx.Products.Add(cmd.Product);
        await _ctx.SaveChangesAsync(ct);
        return cmd.Product;
    }
}