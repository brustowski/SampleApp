using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.PluginEngine.Common;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// Represents Client Address Code data provider
    /// </summary>
    public class ClientAddressCodeDataProvider : ILookupDataProvider, IUiAvailable
    {
        private readonly IClientAddressRepository _repository;

        /// <summary>
        /// Initialize a new instance of the <see cref="ClientAddressCodeDataProvider"/> class
        /// </summary>
        /// <param name="repository">Client address repository</param>
        public ClientAddressCodeDataProvider(IClientAddressRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.ClientAddressCode;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            var lookupItems = new List<LookupItem>();

            if (searchInfo.SearchByKey)
            {
                ClientAddress address = _repository.GetByCode(searchInfo.Search);

                if (address != null)
                {
                    lookupItems.Add(ConvertItem(address));
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

                lookupItems.AddRange(addresses.Select(ConvertItem));
            }

            return lookupItems;
        }

        private static LookupItem ConvertItem(ClientAddress address) => new LookupItem { DisplayValue = address.Code, Value = address.Code };
    }
}