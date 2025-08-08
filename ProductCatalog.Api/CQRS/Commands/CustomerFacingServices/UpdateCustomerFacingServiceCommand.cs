using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Commands.CustomerFacingServices;

/// <summary>
/// Command to update a customer‑facing service.  Nested
/// characteristics and activation sequence are replaced with those
/// provided in the incoming entity.  Returns <c>null</c> if the
/// service does not exist.
/// </summary>
public class UpdateCustomerFacingServiceCommand : ICommand<CustomerFacingService>
{
    public CustomerFacingService Service { get; set; } = null!;
}

public class UpdateCustomerFacingServiceCommandHandler : ICommandHandler<UpdateCustomerFacingServiceCommand, CustomerFacingService>
{
    private readonly ProductCatalogDbContext _ctx;
    public UpdateCustomerFacingServiceCommandHandler(ProductCatalogDbContext ctx) => _ctx = ctx;
    public async Task<CustomerFacingService> Handle(UpdateCustomerFacingServiceCommand cmd, CancellationToken ct)
    {
        var existing = await _ctx.CustomerFacingServices
            .Include(c => c.Characteristics)
            .FirstOrDefaultAsync(c => c.Id == cmd.Service.Id, ct);
        if (existing is null) return null!;
        // Update scalar properties
        _ctx.Entry(existing).CurrentValues.SetValues(cmd.Service);
        // Replace characteristics
        existing.Characteristics.Clear();
        if (cmd.Service.Characteristics != null)
        {
            foreach (var ch in cmd.Service.Characteristics)
            {
                existing.Characteristics.Add(ch);
            }
        }
        // Replace activation sequence
        existing.ActivationSequence = cmd.Service.ActivationSequence ?? new List<string>();
        await _ctx.SaveChangesAsync(ct);
        return existing;
    }
}