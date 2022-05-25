using FilingPortal.DataLayer.Tests.Common;

namespace FilingPortal.Parts.Inbond.DataLayer.Tests
{
    public abstract  class DbTestContext : DbTestContextBase<InbondContext, UnitOfWorkInbondContext>
    {
        protected override UnitOfWorkInbondContext CreateUoW()
        {
            return new UnitOfWorkInbondContext(new InbondContextFactory(), null);
        }
    }
}
