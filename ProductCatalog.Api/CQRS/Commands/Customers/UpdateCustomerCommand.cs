using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProductCatalog.Api.CQRS.Commands.Customers;

public class UpdateCustomerCommand : ICommand<Customer>
{
    public Customer Customer { get; set; } = null!;
}

public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand,Customer>
{
    private readonly ProductCatalogDbContext _ctx;
    public UpdateCustomerCommandHandler(ProductCatalogDbContext ctx)=>_ctx=ctx;
    public async Task<Customer> Handle(UpdateCustomerCommand cmd, CancellationToken ct)
    {
        var existing = await _ctx.Customers.FindAsync(new object[]{cmd.Customer.Id}, ct);
        if(existing is null) return null!;
        _ctx.Entry(existing).CurrentValues.SetValues(cmd.Customer);
        await _ctx.SaveChangesAsync(ct);
        return existing;
    }
}
