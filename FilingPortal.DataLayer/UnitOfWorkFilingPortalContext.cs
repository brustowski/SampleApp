using Framework.DataLayer;
using Framework.Domain.Events;

namespace FilingPortal.DataLayer
{
    /// <summary>
    /// Unit Of Work for <see cref="FilingPortalContext"/>
    /// </summary>
    public class UnitOfWorkFilingPortalContext : UnitOfWork<FilingPortalContext>, IUnitOfWorkDbContext<FilingPortalContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkFilingPortalContext"/> class
        /// </summary>
        /// <param name="contextFactory">The context factory</param>
        /// <param name="eventBus">The event bus</param>
        public UnitOfWorkFilingPortalContext(IDbContextFactory<FilingPortalContext> contextFactory, IEventBusSyncWrapper eventBus)
            : base(contextFactory, eventBus)
        {
        }
    }
}