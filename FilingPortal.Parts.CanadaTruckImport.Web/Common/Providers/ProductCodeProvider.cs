using FilingPortal.Domain.Repositories.Common;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Web.Configs;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Parts.CanadaTruckImport.Web.Common.Providers
{
    /// <summary>
    /// Provider for Product Code
    /// </summary>
    public class ProductCodeDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Repository of the <see cref="ProductCode"/>
        /// </summary>
        private readonly IDataProviderRepository<ProductCode, Guid> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCodeDataProvider"/> class
        /// </summary>
        /// <param name="repository">The repository</param>
        public ProductCodeDataProvider(IDataProviderRepository<ProductCode, Guid> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.ProductCodes;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            var productCodes = new List<ProductCode>();

            if (searchInfo.SearchByKey)
            {
                if (Guid.TryParse(searchInfo.Search, out Guid id))
                {
                    ProductCode productCode = _repository.Get(id);
                    if (productCode != null)
                    {
                        productCodes.Add(productCode);
                    }
                }
            }
            else
            {
                productCodes.AddRange(_repository.Search(searchInfo.Search, searchInfo.Limit));
            }

            return productCodes.Select(productCode => new LookupItem { DisplayValue = $"{productCode.Code} - {productCode.Description}", Value = productCode.Id });
        }
    }
}
