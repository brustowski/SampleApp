using System.Linq;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using FilingPortal.PluginEngine.Common;

namespace FilingPortal.Parts.Zones.Entry.DataLayer
{
    /// <summary>
    /// Inits database
    /// </summary>
    class PluginDatabaseInit : IPluginDatabaseInit
    {
        private readonly UnitOfWorkContext _context;

        /// <summary>
        /// Creates a new instance of <see cref="PluginDatabaseInit"/>
        /// </summary>
        /// <param name="context">Database context</param>
        public PluginDatabaseInit(UnitOfWorkContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Initializes database and applies migrations
        /// </summary>
        public void Init()
        {
            _context.TypedContext.Set<InboundRecord>().Count();
        }
    }
}
