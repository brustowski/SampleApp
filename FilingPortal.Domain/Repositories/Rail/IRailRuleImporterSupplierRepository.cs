using System.Collections.Generic;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Rail
{
    /// <summary>
    /// Describes rail importer-supplier rule repository
    /// </summary>
    public interface IRailRuleImporterSupplierRepository : IRuleRepository<RailRuleImporterSupplier>
    {
        /// <summary>
        /// Gets a list of importer names
        /// </summary>
        /// <param name="search">Search request</param>
        IList<string> GetImporters(string search);

        /// <summary>
        /// Gets a list of supplier names
        /// </summary>
        /// <param name="search">Search request</param>
        IList<string> GetSuppliers(string search);
    }
}
