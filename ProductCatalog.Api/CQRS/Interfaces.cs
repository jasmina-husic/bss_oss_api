namespace ProductCatalog.Api.CQRS;

public interface IQuery<TResult> { }
public interface ICommand<TResult> { }

public interface IQueryHandler<TQuery,TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery query, CancellationToken cancellationToken);
}

public interface ICommandHandler<TCommand,TResult> where TCommand : ICommand<TResult>
{
    Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface IDispatcher
{
    Task<TResult> Query<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
}
