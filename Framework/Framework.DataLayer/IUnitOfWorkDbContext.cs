using Framework.Domain.Repositories;

namespace Framework.DataLayer
{
    public interface IUnitOfWorkDbContext : IUnitOfWork
    {
        DbExtendedContext Context { get; }
    }

    public interface IUnitOfWorkDbContext<out TContext> : IUnitOfWorkDbContext
        where TContext : DbExtendedContext

    {
        TContext TypedContext { get; }
    }
}