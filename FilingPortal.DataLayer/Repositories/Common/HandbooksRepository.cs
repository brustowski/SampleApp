using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using FilingPortal.Domain.Repositories.Common;
using Framework.DataLayer;
using Framework.Domain.Paging;
using Framework.Infrastructure;

namespace FilingPortal.DataLayer.Repositories.Common
{
    /// <summary>
    /// Implements handbooks repository
    /// </summary>
    internal class HandbooksRepository : IHandbookRepository
    {

        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        /// <summary>
        /// Database context
        /// </summary>
        private IUnitOfWorkDbContext UnitOfWork => _unitOfWorkFactory.Create();

        /// <summary>
        /// Creates instance of handbooks repository
        /// </summary>
        /// <param name="unitOfWorkFactory">Factory of databases</param>
        public HandbooksRepository(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Returns list of available handbooks
        /// </summary>
        public IEnumerable<string> GetHandbooks()
        {
            const string command = @"SELECT * FROM v_Handbooks";

            var context = (FilingPortalContext)UnitOfWork.Context;
            using (new MonitoredScope($"Get handbooks list"))
            {
                return context.Database.SqlQuery<string>(command).ToList();
            }
        }

        /// <summary>
        /// Returns available options for handbook
        /// </summary>
        /// <param name="handbookName">Handbook name</param>
        /// <param name="searchInfoSearch">Search request</param>
        /// <param name="searchInfoLimit">Result items limit</param>
        public IList<LookupItem<string>> GetOptions(string handbookName, string searchInfoSearch, int? searchInfoLimit)
        {
            const string command = @"EXEC dbo.app_get_handbook @name, @search, @limit";

            var context = (FilingPortalContext)UnitOfWork.Context;
            using (new MonitoredScope($"Execute dbo.app_get_handbook with param {handbookName}"))
            {
                return context.Database.SqlQuery<LookupItem<string>>(command,
                    new SqlParameter("name", handbookName),
                    new SqlParameter("search", searchInfoSearch ?? (object)DBNull.Value),
                    new SqlParameter("limit", searchInfoLimit ?? (object)DBNull.Value))
                .ToList();
            }
        }
    }
}
