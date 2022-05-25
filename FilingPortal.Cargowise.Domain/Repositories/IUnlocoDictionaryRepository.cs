using System.Collections.Generic;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using Framework.Domain.Repositories;

namespace FilingPortal.Cargowise.Domain.Repositories
{
    /// <summary>
    /// Describes UNLOCO repository
    /// </summary>
    public interface IUnlocoDictionaryRepository : ISearchRepository<UnlocoDictionaryEntry>
    {
        /// <summary>
        /// Searches for UNLOCO
        /// </summary>
        /// <param name="searchInfoSearch">Search request</param>
        /// <param name="searchInfoLimit">Max items limit</param>
        IList<UnlocoDictionaryEntry> Search(string searchInfoSearch, int searchInfoLimit);

        /// <summary>
        /// Searches for UNLOCO specified by country code
        /// </summary>
        /// <param name="searchInfoSearch">Search request</param>
        /// <param name="searchInfoLimit">Max items limit</param>
        /// <param name="countryCode">Country code</param>
        IList<UnlocoDictionaryEntry> Search(string searchInfoSearch, int searchInfoLimit, string countryCode);

        /// <summary>
        /// Gets dictionary entry by UNLOCO code
        /// </summary>
        /// <param name="searchInfoSearch">Search request</param>
        UnlocoDictionaryEntry GetByCode(string searchInfoSearch);
    }
}
