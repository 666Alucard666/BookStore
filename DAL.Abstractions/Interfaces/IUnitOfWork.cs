using Core.Models;

namespace DAL.Abstractions.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepos<User> UserRepository { get; }
    IGenericRepos<Book> BookRepository { get; }
    IGenericRepos<Order> OrderRepository { get; }
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}