using FilingPortal.Domain.Repositories.Common;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using System;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Repositories
{
    /// <summary>
    /// Describes the Product Code repository
    /// </summary>
    public interface IProductCodeRepository : IDataProviderRepository<ProductCode, Guid>
    {
        /// <summary>
        /// Checks if product with specified code exists
        /// </summary>
        /// <param name="productCode">The product code to check</param>
        bool IsExist(string productCode);

        /// <summary>
        /// Gets the Product by specified code
        /// </summary>
        /// <param name="productCode">The Product code</param>
        ProductCode GetByCode(string productCode);
    }
}
