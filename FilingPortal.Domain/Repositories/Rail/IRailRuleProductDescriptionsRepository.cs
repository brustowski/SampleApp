using System.Collections.Generic;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Rail
{
    /// <summary>
    /// Describes rail product descriptions rule repository
    /// </summary>
    public interface IRailRuleProductDescriptionsRepository : IRuleRepository<RailRuleDescription>
    {
        /// <summary>
        /// Gets a list of descriptions
        /// </summary>
        /// <param name="search">Search request</param>
        IList<string> GetDescriptions(string search);
    }
}
