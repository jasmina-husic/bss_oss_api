using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Commands.CustomerFacingServices;

/// <summary>
/// Command to create a new customer‑facing service.  Any provided
/// characteristics and activation sequence are stored along with
/// the service.  Returns the persisted entity with generated Id.
/// </summary>
public class CreateCustomerFacingServiceCommand : ICommand<CustomerFacingService>
{
    public CustomerFacingService Service { get; set; } = null!;
}

public class CreateCustomerFacingServiceCommandHandler : ICommandHandler<CreateCustomerFacingServiceCommand, CustomerFacingService>
{
    private readonly ProductCatalogDbContext _ctx;
    public CreateCustomerFacingServiceCommandHandler(ProductCatalogDbContext ctx) => _ctx = ctx;
    public async Task<CustomerFacingService> Handle(CreateCustomerFacingServiceCommand cmd, CancellationToken ct)
    {
        if (cmd.Service.Id != 0)
            cmd.Service.Id = 0;
        _ctx.CustomerFacingServices.Add(cmd.Service);
        await _ctx.SaveChangesAsync(ct);
        return cmd.Service;
    }
}