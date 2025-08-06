using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;

namespace ProductCatalog.Api.CQRS.Commands.Customers;

public class DeleteCustomerCommand : ICommand<bool>
{
    public int Id { get; set; }
}

public class DeleteCustomerCommandHandler
        : ICommandHandler<DeleteCustomerCommand, bool>
{
    private readonly ProductCatalogDbContext _ctx;
    public DeleteCustomerCommandHandler(ProductCatalogDbContext ctx) => _ctx = ctx;

    public async Task<bool> Handle(DeleteCustomerCommand cmd, CancellationToken ct)
    {
        var customer = await _ctx.Customers
                                 .Include(c => c.BillingAddress)
                                 .Include(c => c.ShippingAddress)
                                 .FirstOrDefaultAsync(c => c.Id == cmd.Id, ct);

        if (customer is null) return false;

        // Soft-delete: flip the flag only – no Remove(), no NULL columns
        customer.IsDeleted = true;
        customer.LastModified = DateTime.UtcNow;

        await _ctx.SaveChangesAsync(ct);
        return true;
    }
}
