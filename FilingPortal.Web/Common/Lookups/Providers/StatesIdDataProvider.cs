using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Repositories;
using Framework.Domain.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provides list of state id/title pairs
    /// </summary>
    public class StatesIdDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// USStates tables repository
        /// </summary>
        protected readonly IUSStatesRepository<USStates> Repository;

        /// <summary>
        /// Initialize a new instance of the <see cref="StatesDataProvider"/> class
        /// </summary>
        /// <param name="repository"> US_States repository </param>
        public StatesIdDataProvider(IUSStatesRepository<USStates> repository) => Repository = repository;

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public virtual string Name => DataProviderNames.StateIds;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public virtual IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (searchInfo.SearchByKey && !string.IsNullOrWhiteSpace(searchInfo.Search))
            {
                USStates record = Repository.Get(Convert.ToInt32(searchInfo.Search));
                var result = new List<LookupItem>();
                if (record != null)
                {
                    result.Add(new LookupItem { DisplayValue = record.StateName + " - " + record.StateCode, Value = record.Id });
                }

                return result;
            }

            IEnumerable<USStates> data = Repository.GetStateData(searchInfo.Search, searchInfo.Limit);
            return data.Select(x => new LookupItem { DisplayValue = x.StateName + " - " + x.StateCode, Value = x.Id });
        }
    }

    /// <summary>
    /// Provides list of state id/title pairs with Non Applicable option
    /// </summary>
    public class VesselImportsStatesIdDataProvider : StatesIdDataProvider
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="StatesDataProvider"/> class
        /// </summary>
        /// <param name="repository"> US_States repository </param>
        public VesselImportsStatesIdDataProvider(IUSStatesRepository<USStates> repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public override string Name => DataProviderNames.VesselImportsStateIds;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public override IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            var results = base.GetData(searchInfo).ToList();
            if (string.IsNullOrWhiteSpace(searchInfo.Search))
                results.Insert(0, new LookupItem {Value = null, DisplayValue = "Not applicable"});
            return results;
        }
    }
}