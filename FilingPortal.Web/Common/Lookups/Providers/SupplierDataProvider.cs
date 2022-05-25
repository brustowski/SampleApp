using System;
using FilingPortal.Domain.Repositories.Clients;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for Supplier lookup data provider
    /// </summary>
    public class SupplierDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Client tables repository
        /// </summary>
        private readonly IClientRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SupplierDataProvider"/> class
        /// </summary>
        /// <param name="repository">Client table repository</param>
        public SupplierDataProvider(IClientRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.Suppliers;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>

        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (searchInfo.SearchByKey && !string.IsNullOrWhiteSpace(searchInfo.Search))
            {
                Client record = _repository.Get(new Guid(searchInfo.Search));
                var result = new List<LookupItem>();
                if (record != null)
                    result.Add(new LookupItem { DisplayValue =  $"{record.ClientCode} - {record.ClientName}", Value = record.Id });
                return result;
            }

            IEnumerable<Client> data = _repository.GetSuppliers(searchInfo.Search, searchInfo.Limit);
            return data.Select(x => new LookupItem { DisplayValue = $"{x.ClientCode} - {x.ClientName}", Value = x.Id });
        }
    }
}