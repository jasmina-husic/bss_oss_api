using Microsoft.Extensions.DependencyInjection;
namespace ProductCatalog.Api.CQRS;

public class Dispatcher : IDispatcher
{
    private readonly IServiceProvider _sp;
    public Dispatcher(IServiceProvider sp) => _sp = sp;

    public async Task<TResult> Query<TResult>(IQuery<TResult> query, CancellationToken ct = default)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        dynamic handler = _sp.GetRequiredService(handlerType);
        return await handler.Handle((dynamic)query, ct);
    }

    public async Task<TResult> Send<TResult>(ICommand<TResult> cmd, CancellationToken ct = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(cmd.GetType(), typeof(TResult));
        dynamic handler = _sp.GetRequiredService(handlerType);
        return await handler.Handle((dynamic)cmd, ct);
    }
}
