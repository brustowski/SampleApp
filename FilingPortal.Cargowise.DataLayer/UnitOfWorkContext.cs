using Framework.DataLayer;
using Framework.Domain.Events;

namespace FilingPortal.Cargowise.DataLayer
{
    /// <summary>
    /// Unit Of Work for <see cref="UnitOfWorkContext"/>
    /// </summary>
    public class UnitOfWorkContext : UnitOfWork<CargoWiseContext>, IUnitOfWorkDbContext<CargoWiseContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkContext"/> class
        /// </summary>
        /// <param name="contextFactory">The context factory</param>
        /// <param name="eventBus">The event bus</param>
        public UnitOfWorkContext(IDbContextFactory<CargoWiseContext> contextFactory, IEventBusSyncWrapper eventBus)
            : base(contextFactory, eventBus)
        {
        }
    }
}