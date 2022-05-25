using Framework.DataLayer;

namespace FilingPortal.DataLayer.Tests.Common
{
    public class TestUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private IUnitOfWorkDbContext _unitOfWork;

        public IUnitOfWorkDbContext Create()
        {
            return _unitOfWork ?? (_unitOfWork = new UnitOfWorkFilingPortalContext(new FilingPortalContextFactory(), null));
        }
    }


}