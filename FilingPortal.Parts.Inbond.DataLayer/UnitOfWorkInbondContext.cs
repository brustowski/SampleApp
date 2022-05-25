using Framework.DataLayer;
using Framework.Domain.Events;

namespace FilingPortal.Parts.Inbond.DataLayer
{
    /// <summary>
    /// Unit Of Work for <see cref="InbondContext"/>
    /// </summary>
    public class UnitOfWorkInbondContext : UnitOfWork<InbondContext>, IUnitOfWorkDbContext<InbondContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkInbondContext"/> class
        /// </summary>
        /// <param name="contextFactory">The context factory</param>
        /// <param name="eventBus">The event bus</param>
        public UnitOfWorkInbondContext(IDbContextFactory<InbondContext> contextFactory, IEventBusSyncWrapper eventBus)
            : base(contextFactory, eventBus)
        {
        }
    }
}