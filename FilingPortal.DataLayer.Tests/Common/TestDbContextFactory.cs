using System.Data.Entity;
using Framework.DataLayer.ReadModel;

namespace FilingPortal.DataLayer.Tests.Common
{
    class TestDbContextFactory : IContextFactory
    {
        public DbContext Create()
        {
            return new FilingPortalContext();
        }
    }
}
