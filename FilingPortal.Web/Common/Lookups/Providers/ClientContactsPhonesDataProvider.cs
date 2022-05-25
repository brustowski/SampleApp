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
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for client contacts phones
    /// </summary>
    public class ClientContactsPhonesDataProvider : IEditableLookupDataProvider
    {
        /// <summary>
        /// Contacts repository
        /// </summary>
        private readonly IClientContactsRepository _contactsRepository;

        /// <summary>
        /// Creates a new instance of <see cref="ClientContactsPhonesDataProvider"/>
        /// </summary>
        /// <param name="contactsRepository">Contacts repository</param>
        public ClientContactsPhonesDataProvider(IClientContactsRepository contactsRepository)
        {
            _contactsRepository = contactsRepository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.ContactPhones;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            var lookupItems = new List<LookupItem>();

            if (!string.IsNullOrWhiteSpace(searchInfo.DependOn))
            {
                var contact = Guid.TryParse(searchInfo.DependValue, out Guid contactId)
                    ? _contactsRepository.Get(contactId)
                    : null;
                if (contact != null)
                {
                    lookupItems.AddIf(!string.IsNullOrEmpty(contact.WorkPhone),
                        contact.WorkPhone.Map<string, LookupItem>());
                    lookupItems.AddIf(!string.IsNullOrEmpty(contact.MobilePhone),
                        contact.MobilePhone.Map<string, LookupItem>());
                    lookupItems.AddIf(!string.IsNullOrEmpty(contact.HomePhone),
                        contact.HomePhone.Map<string, LookupItem>());
                    lookupItems.AddRange(contact.AdditionalPhones.Select(x => x.Map<ClientContactAdditionalPhone, LookupItem>()));

                    if (lookupItems.Any() && lookupItems.All(x => !x.IsDefault)) lookupItems.First().IsDefault = true;
                }
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
            if (dependValue != null && Guid.TryParse(dependValue.ToString(), out Guid contactId))
            {
                var contact = _contactsRepository.Get(contactId);
                if (contact != null)
                {
                    contact.AdditionalPhones.Add(new ClientContactAdditionalPhone { Phone = modelValue, ContactId = contactId });
                    _contactsRepository.Save();
                    return new LookupItem
                    {
                        Value = modelValue,
                        DisplayValue = modelValue
                    };
                }
            }

            throw new ArgumentException(@"Can't create phone, wrong contact ID", nameof(dependValue));
        }
    }
}