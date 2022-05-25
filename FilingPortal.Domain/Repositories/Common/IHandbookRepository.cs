using System.Collections.Generic;
using Framework.Domain.Paging;

namespace FilingPortal.Domain.Repositories.Common
{
    /// <summary>
    /// Describes handbooks repository
    /// </summary>
    public interface IHandbookRepository
    {
        /// <summary>
        /// Returns list of available handbooks
        /// </summary>
        IEnumerable<string> GetHandbooks();

        /// <summary>
        /// Returns available options for handbook
        /// </summary>
        /// <param name="handbookName">Handbook name</param>
        /// <param name="searchInfoSearch">Search request</param>
        /// <param name="searchInfoLimit">Result items limit</param>
        IList<LookupItem<string>> GetOptions(string handbookName, string searchInfoSearch = null, int? searchInfoLimit = null);
    }
}
