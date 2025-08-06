using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Commands.Customers;

/// <summary>
/// Updates a customer identified by its external CRM Id WITHOUT overwriting the
/// stored CrmId when the client omits it (or sends null / empty).
/// </summary>
public class UpdateCustomerByCrmIdCommand : ICommand<Customer?>
{
    public string CrmId { get; set; } = string.Empty;
    public Customer Customer { get; set; } = null!;
}

public class UpdateCustomerByCrmIdCommandHandler
        : ICommandHandler<UpdateCustomerByCrmIdCommand, Customer?>
{
    private readonly ProductCatalogDbContext _ctx;
    public UpdateCustomerByCrmIdCommandHandler(ProductCatalogDbContext ctx) => _ctx = ctx;

    public async Task<Customer?> Handle(UpdateCustomerByCrmIdCommand cmd,
                                        CancellationToken ct)
    {
        var existing = await _ctx.Customers
                                 .Include(c => c.BillingAddress)
                                 .Include(c => c.ShippingAddress)
                                 .FirstOrDefaultAsync(c => c.CrmId == cmd.CrmId, ct);

        if (existing is null) return null;

        // Copy scalar values EXCEPT CrmId
        var incomingValues = _ctx.Entry(cmd.Customer).CurrentValues;
        incomingValues[nameof(Customer.CrmId)] = existing.CrmId; 

        _ctx.Entry(existing).CurrentValues.SetValues(incomingValues);

        if (cmd.Customer.BillingAddress is not null)
        {
            var billing = _ctx.Entry(existing)
                              .Reference(c => c.BillingAddress)
                              .TargetEntry;
            billing.CurrentValues.SetValues(cmd.Customer.BillingAddress);
        }

        if (cmd.Customer.ShippingAddress is not null)
        {
            var ship = _ctx.Entry(existing)
                           .Reference(c => c.ShippingAddress)
                           .TargetEntry;
            ship.CurrentValues.SetValues(cmd.Customer.ShippingAddress);
        }

        await _ctx.SaveChangesAsync(ct);
        return existing;
    }
}