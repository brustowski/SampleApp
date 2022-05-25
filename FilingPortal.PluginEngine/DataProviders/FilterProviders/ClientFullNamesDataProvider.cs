﻿using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.PluginEngine.DataProviders.FilterProviders
{
    /// <summary>
    /// Provider for importer lookup data provider with long title in value
    /// </summary>
    public class ClientFullNamesDataProvider : IFilterDataProvider
    {
        /// <summary>
        /// Client tables repository
        /// </summary>
        private readonly IClientRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientFullNamesDataProvider"/> class
        /// </summary>
        /// <param name="repository">Client table repository</param>
        public ClientFullNamesDataProvider(IClientRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>

        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (searchInfo.SearchByKey && !string.IsNullOrWhiteSpace(searchInfo.Search))
            {
                Client record = _repository.GetAll(searchInfo.Search, 1).FirstOrDefault();
                var result = new List<LookupItem>();
                if (record != null)
                {
                    result.Add(new LookupItem { DisplayValue = $"{record.ClientCode} - {record.ClientName}", Value = record.ClientName });
                }

                return result;
            }

            IEnumerable<Client> data = _repository.GetAll(searchInfo.Search, searchInfo.Limit);
            return data.Select(x => new LookupItem { DisplayValue = $"{x.ClientCode} - {x.ClientName}", Value = x.ClientName });
        }
    }
}