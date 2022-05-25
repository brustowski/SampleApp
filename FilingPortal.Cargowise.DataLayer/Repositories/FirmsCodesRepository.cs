using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Cargowise.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Cargowise.DataLayer.Repositories
{
    /// <summary>
    /// Implements methods working with FIRMs codes
    /// </summary>
    internal class FirmsCodesRepository : SearchRepository<CargowiseFirmsCodes>, IFirmsCodesRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="FirmsCodesRepository"/>
        /// </summary>
        /// <param name="unitOfWork">Unit of work</param>
        public FirmsCodesRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Searches for FIRMs codes in repository
        /// </summary>
        /// <param name="searchInfoSearch">Search request</param>
        /// <param name="searchInfoLimit">Max items limit</param>
        /// <param name="stateId">State Id</param>
        public IList<CargowiseFirmsCodes> Search(string searchInfoSearch, int searchInfoLimit, int? stateId)
        {
            IQueryable<CargowiseFirmsCodes> query = Set.Where(x => x.StateId == stateId);
            if (!string.IsNullOrWhiteSpace(searchInfoSearch))
            {
                query = query.Where(x => x.Name.Contains(searchInfoSearch) || x.FirmsCode.Contains(searchInfoSearch));
            }

            return query.OrderBy(x => x.FirmsCode).Take(searchInfoLimit).ToList();
        }

        /// <summary>
        /// Searches for FIRMs Codes in repository
        /// </summary>
        /// <param name="searchInfoSearch">Search request</param>
        /// <param name="searchInfoLimit">Max items limit</param>
        public IList<CargowiseFirmsCodes> Search(string searchInfoSearch, int searchInfoLimit)
        {
            IQueryable<CargowiseFirmsCodes> query = Set;
            if (!string.IsNullOrWhiteSpace(searchInfoSearch))
            {
                query = query.Where(x => x.Name.Contains(searchInfoSearch) || x.FirmsCode.Contains(searchInfoSearch));
            }

            return query.Take(searchInfoLimit).OrderBy(x => x.FirmsCode).ToList();
        }

        /// <summary>
        /// Checks whether record with specified FIRMs code exists in the table
        /// </summary>
        /// <param name="firmsCode">The FIRMs code</param>
        public bool IsExist(string firmsCode)
        {
            return Set.Any(x => x.FirmsCode.Equals(firmsCode));
        }

        /// <summary>
        /// Returns FIRMs code if exists
        /// </summary>
        /// <param name="firmsCode">The FIRMs code</param>
        public CargowiseFirmsCodes GetByCode(string firmsCode) =>
            Set.FirstOrDefault(x =>
                x.FirmsCode.Equals(firmsCode, StringComparison.InvariantCultureIgnoreCase));
    }
}
