using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// Provider for client contacts code
    /// </summary>
    public class ClientContactCodeDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Contacts repository
        /// </summary>
        private readonly IClientContactsRepository _contactsRepository;

        /// <summary>
        /// Creates a new instance of <see cref="ClientContactCodeDataProvider"/>
        /// </summary>
        /// <param name="contactsRepository">Country repository</param>
        public ClientContactCodeDataProvider(IClientContactsRepository contactsRepository)
        {
            _contactsRepository = contactsRepository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.ClientContactCode;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            var lookupItems = new List<LookupItem>();

            if (searchInfo.SearchByKey)
            {
                ClientContact contact = _contactsRepository.GetByCode(searchInfo.Search);
                if (contact != null)
                {
                    lookupItems.Add(ConvertItem(contact));
                }
            }
            else
            {
                IEnumerable<ClientContact> contacts;

                if (string.IsNullOrWhiteSpace(searchInfo.DependOn))
                {
                    contacts = _contactsRepository.Search(searchInfo.Search, searchInfo.Limit);
                }
                else
                {
                    contacts = Guid.TryParse(searchInfo.DependValue, out Guid clientId)
                        ? _contactsRepository.Search(searchInfo.Search, searchInfo.Limit, clientId)
                        : _contactsRepository.Search(searchInfo.Search, searchInfo.Limit, searchInfo.DependValue);
                }

                lookupItems.AddRange(contacts.Select(ConvertItem));
            }

            return lookupItems;
        }

        private static LookupItem ConvertItem(ClientContact contact)
            => new LookupItem { DisplayValue = contact.ContactName, Value = contact.ContactName };
    }
}