using FilingPortal.Domain.Repositories.Clients;
using Framework.Domain.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Represents Client Addresses data provider
    /// </summary>
    public class ClientAddressDataProvider : ILookupDataProvider
    {
        private readonly IClientAddressRepository _repository;

        /// <summary>
        /// Initialize a new instance of the <see cref="ClientAddressDataProvider"/> class
        /// </summary>
        /// <param name="repository">Client address repository</param>
        public ClientAddressDataProvider(IClientAddressRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.ClientAddresses;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            var lookupItems = new List<LookupItem>(); 

            if (searchInfo.SearchByKey)
            {
                ClientAddress address = Guid.TryParse(searchInfo.Search, out Guid addressId) 
                    ? _repository.Get(addressId) 
                    : null;

                if (address != null)
                {
                    lookupItems.Add(new LookupItem { DisplayValue = address.Code, Value = address.Id });
                }
            }
            else
            {
                IEnumerable<ClientAddress> addresses;

                if (string.IsNullOrWhiteSpace(searchInfo.DependOn))
                {
                    addresses = _repository.Search(searchInfo.Search, searchInfo.Limit);
                }
                else
                {
                    addresses = Guid.TryParse(searchInfo.DependValue, out Guid clientId) 
                        ? _repository.Search(searchInfo.Search, searchInfo.Limit, clientId) 
                        : _repository.Search(searchInfo.Search, searchInfo.Limit, searchInfo.DependValue);
                }

                lookupItems.AddRange(addresses.Select(x => new LookupItem { DisplayValue = x.Code, Value = x.Id }));
            }

            return lookupItems;
        }
    }
}