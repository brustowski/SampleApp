using System.Collections.Generic;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using Framework.Domain.Repositories;

namespace FilingPortal.Cargowise.Domain.Repositories
{
    /// <summary>
    /// Describes methods working with FIRMs codes
    /// </summary>
    public interface IFirmsCodesRepository : ISearchRepository<CargowiseFirmsCodes>
    {
        /// <summary>
        /// Searches for FIRMs Codes in repository
        /// </summary>
        /// <param name="searchInfoSearch">Search request</param>
        /// <param name="searchInfoLimit">Max items limit</param>
        /// <param name="stateId">State to search within</param>
        IList<CargowiseFirmsCodes> Search(string searchInfoSearch, int searchInfoLimit, int? stateId);

        /// <summary>
        /// Searches for FIRMs Codes in repository
        /// </summary>
        /// <param name="searchInfoSearch">Search request</param>
        /// <param name="searchInfoLimit">Max items limit</param>
        IList<CargowiseFirmsCodes> Search(string searchInfoSearch, int searchInfoLimit);

        /// <summary>
        /// Checks whether record with specified FIRMs code exists in the table
        /// </summary>
        /// <param name="firmsCode">The FIRMs code</param>
        bool IsExist(string firmsCode);

        /// <summary>
        /// Returns FIRMs code if exists
        /// </summary>
        /// <param name="firmsCode">The FIRMs code</param>
        CargowiseFirmsCodes GetByCode(string firmsCode);
    }
}
