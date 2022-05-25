using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Repositories.Common;
using Framework.Domain.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for Vessel data 
    /// </summary>
    public class VesselIdDataProvider : IEditableLookupDataProvider
    {
        /// <summary>
        /// Vessel handbook repository
        /// </summary>
        private readonly IVesselRepository _repository;

        /// <summary>
        /// Initialize Vessel handbook repository
        /// </summary>
        /// <param name="repository">Tariff table Repository</param>
        public VesselIdDataProvider(IVesselRepository repository) => _repository = repository;

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.Vessels;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (searchInfo.SearchByKey && !string.IsNullOrWhiteSpace(searchInfo.Search))
            {
                VesselHandbookRecord record = _repository.Get(Convert.ToInt32(searchInfo.Search));
                var result = new List<LookupItem>();
                if (record != null)
                {
                    result.Add(new LookupItem { DisplayValue = record.Name, Value = record.Id });
                }

                return result;
            }

            IEnumerable<VesselHandbookRecord> data = _repository.SearchVessel(searchInfo.Search, searchInfo.Limit);
            return data.Select(x => new LookupItem { DisplayValue = x.Name, Value = x.Id });
        }
        /// <summary>
        /// Adds new value to handbook
        /// </summary>
        /// <param name="modelValue">Option value</param>
        /// <param name="dependValue">Additional data</param>
        public LookupItem Add(string modelValue, object dependValue)
        {
            var newRecord = new VesselHandbookRecord { Name = modelValue };
            _repository.Add(newRecord);
            _repository.Save();
            return new LookupItem { DisplayValue = newRecord.Name, Value = newRecord.Id };
        }
    }
}