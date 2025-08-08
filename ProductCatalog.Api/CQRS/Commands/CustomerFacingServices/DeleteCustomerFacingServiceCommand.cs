using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;

namespace ProductCatalog.Api.CQRS.Commands.CustomerFacingServices;

/// <summary>
/// Command to soft‑delete a customer‑facing service.  Returns true
/// when the service is found and marked deleted; otherwise false.
/// </summary>
public class DeleteCustomerFacingServiceCommand : ICommand<bool>
{
    public int Id { get; set; }
}

public class DeleteCustomerFacingServiceCommandHandler : ICommandHandler<DeleteCustomerFacingServiceCommand, bool>
{
    private readonly ProductCatalogDbContext _ctx;
    public DeleteCustomerFacingServiceCommandHandler(ProductCatalogDbContext ctx) => _ctx = ctx;
    public async Task<bool> Handle(DeleteCustomerFacingServiceCommand cmd, CancellationToken ct)
    {
        var svc = await _ctx.CustomerFacingServices.FirstOrDefaultAsync(c => c.Id == cmd.Id, ct);
        if (svc is null) return false;
        svc.IsDeleted = true;
        svc.LastModified = DateTime.UtcNow;
        await _ctx.SaveChangesAsync(ct);
        return true;
    }
}