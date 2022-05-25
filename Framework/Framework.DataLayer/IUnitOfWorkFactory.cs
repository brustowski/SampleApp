using System.Data.Entity;

namespace Framework.DataLayer
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWorkDbContext Create();
    }

    public interface IUnitOfWorkFactory<out TContext>: IUnitOfWorkFactory where TContext : IUnitOfWorkDbContext
    {
    }
}