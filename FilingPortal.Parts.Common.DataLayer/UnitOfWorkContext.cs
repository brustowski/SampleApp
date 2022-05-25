using Framework.DataLayer;
using Framework.Domain.Events;

namespace FilingPortal.Parts.Common.DataLayer
{
    /// <summary>
    /// Unit Of Work for <see cref="UnitOfWorkContext"/>
    /// </summary>
    public class UnitOfWorkContext : UnitOfWork<CommonContext>, IUnitOfWorkDbContext<CommonContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkContext"/> class
        /// </summary>
        /// <param name="contextFactory">The context factory</param>
        /// <param name="eventBus">The event bus</param>
        public UnitOfWorkContext(IDbContextFactory<CommonContext> contextFactory, IEventBusSyncWrapper eventBus)
            : base(contextFactory, eventBus)
        {
        }
    }
}