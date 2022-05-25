using Framework.DataLayer;
using Framework.Domain.Events;

namespace FilingPortal.Parts.Zones.Ftz214.DataLayer
{
    /// <summary>
    /// Unit Of Work for <see cref="UnitOfWorkContext"/>
    /// </summary>
    public class UnitOfWorkContext : UnitOfWork<PluginContext>, IUnitOfWorkDbContext<PluginContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkContext"/> class
        /// </summary>
        /// <param name="contextFactory">The context factory</param>
        /// <param name="eventBus">The event bus</param>
        public UnitOfWorkContext(IDbContextFactory<PluginContext> contextFactory, IEventBusSyncWrapper eventBus)
            : base(contextFactory, eventBus)
        {
        }
    }
}