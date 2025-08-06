using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;

namespace ProductCatalog.Api.CQRS.Commands.Customers;

public class DeleteCustomerByCrmIdCommand : ICommand<bool>
{
    public string CrmId { get; set; } = string.Empty;
}

public class DeleteCustomerByCrmIdCommandHandler : ICommandHandler<DeleteCustomerByCrmIdCommand,bool>
{
    private readonly ProductCatalogDbContext _ctx;
    public DeleteCustomerByCrmIdCommandHandler(ProductCatalogDbContext ctx)=>_ctx=ctx;

    public async Task<bool> Handle(DeleteCustomerByCrmIdCommand cmd, CancellationToken ct)
    {
        var customer = await _ctx.Customers.FirstOrDefaultAsync(c=>c.CrmId==cmd.CrmId, ct);
        if(customer is null) return false;

        customer.IsDeleted = true;
        customer.LastModified = DateTime.UtcNow;
        await _ctx.SaveChangesAsync(ct);
        return true;
    }
}