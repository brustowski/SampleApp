using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for client contacts
    /// </summary>
    public class ClientContactsDataProvider : IEditableLookupDataProvider
    {
        /// <summary>
        /// Contacts repository
        /// </summary>
        private readonly IClientContactsRepository _contactsRepository;
        /// <summary>
        /// Clients repository
        /// </summary>
        private readonly IClientRepository _clientRepository;

        /// <summary>
        /// Creates a new instance of <see cref="ClientContactsDataProvider"/>
        /// </summary>
        /// <param name="contactsRepository">Country repository</param>
        /// <param name="clientRepository">Clients repository</param>
        public ClientContactsDataProvider(IClientContactsRepository contactsRepository, IClientRepository clientRepository)
        {
            _contactsRepository = contactsRepository;
            _clientRepository = clientRepository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.ClientContacts;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            var lookupItems = new List<LookupItem>();

            if (searchInfo.SearchByKey)
            {
                if (Guid.TryParse(searchInfo.Search, out Guid contactId))
                {
                    ClientContact contact = _contactsRepository.Get(contactId);
                    if (contact != null)
                    {
                        lookupItems.Add(contact.Map<ClientContact, LookupItem>());
                    }
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
                        : Enumerable.Empty<ClientContact>();
                }

                lookupItems.AddRange(contacts.Select(x => x.Map<ClientContact, LookupItem>()));
            }

            return lookupItems;
        }

        /// <summary>
        /// Adds new value to handbook
        /// </summary>
        /// <param name="modelValue">Option value</param>
        /// <param name="dependValue">Additional data</param>
        public LookupItem Add(string modelValue, object dependValue = null)
        {
            if (dependValue != null && Guid.TryParse(dependValue.ToString(), out Guid clientId))
            {
                Client contact = _clientRepository.Get(clientId);
                if (contact != null)
                {
                    var newContact = new ClientContact { ContactName = modelValue, ClientId = clientId };
                    _contactsRepository.Add(newContact);
                    _contactsRepository.Save();
                    return new LookupItem
                    {
                        Value = newContact.Id,
                        DisplayValue = newContact.ContactName
                    };
                }
            }

            throw new ArgumentException(@"Can't create contact, wrong client ID", nameof(dependValue));
        }
    }
}