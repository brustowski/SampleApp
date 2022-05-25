using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Cargowise.Domain.Repositories
{
    /// <summary>
    /// Describe Country repository
    /// </summary>
    public interface ICountryRepository : IDataProviderRepository<Country, int>
    {
        /// <summary>
        /// Gets country by country code
        /// </summary>
        /// <param name="code">The country code</param>
        Country GetByCode(string code);
    }
}