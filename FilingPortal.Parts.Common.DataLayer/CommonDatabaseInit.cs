using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities;
using FilingPortal.PluginEngine.Common;

namespace FilingPortal.Parts.Common.DataLayer
{
    /// <summary>
    /// Inits database
    /// </summary>
    class CommonDatabaseInit : IPluginDatabaseInit
    {
        private readonly UnitOfWorkContext _context;

        /// <summary>
        /// Creates a new instance of <see cref="CommonDatabaseInit"/>
        /// </summary>
        /// <param name="context">Database context</param>
        public CommonDatabaseInit(UnitOfWorkContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Initializes database and applies migrations
        /// </summary>
        public void Init()
        {
            _context.TypedContext.Set<HeaderJobStatus>().Count();
        }
    }
}
