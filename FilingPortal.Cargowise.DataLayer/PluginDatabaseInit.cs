using System.Linq;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.PluginEngine.Common;

namespace FilingPortal.Cargowise.DataLayer
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
            _context.TypedContext.Set<USStates>().Count();
        }
    }
}
