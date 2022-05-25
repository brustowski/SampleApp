using Framework.DataLayer;

namespace FilingPortal.DataLayer.Tests.Common
{
    public abstract class DbTestContextBase<TContext, TUoWContext>
        where TContext : DbExtendedContext
        where TUoWContext : IUnitOfWorkDbContext<TContext>
    {
        public readonly TUoWContext UnitOfWork;
        public readonly TContext DbContext;

        protected DbTestContextBase()
        {
            UnitOfWork = CreateUoW();
            DbContext = (TContext)UnitOfWork.Context;
            DbContext.Database.CreateIfNotExists();
        }

        protected abstract TUoWContext CreateUoW();
    }
}
