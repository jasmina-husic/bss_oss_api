using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Commands.Customers;

public class CreateCustomerCommand : ICommand<Customer>
{
    public Customer Customer { get; set; } = null!;
}

public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, Customer>
{
    private readonly ProductCatalogDbContext _ctx;
    public CreateCustomerCommandHandler(ProductCatalogDbContext ctx) => _ctx = ctx;

    public async Task<Customer> Handle(CreateCustomerCommand cmd, CancellationToken ct)
    {
        // If the client sent "id": X, clear it so EF generates the key.
        if (cmd.Customer.Id != 0)
            cmd.Customer.Id = 0;

        _ctx.Customers.Add(cmd.Customer);
        await _ctx.SaveChangesAsync(ct);
        return cmd.Customer;
    }
}
