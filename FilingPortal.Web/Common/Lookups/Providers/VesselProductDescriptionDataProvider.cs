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
    /// Provider for Discharge terminal data 
    /// </summary>
    public class VesselProductDescriptionDataProvider : IEditableLookupDataProvider
    {
        /// <summary>
        /// Product Descriptions handbook repository
        /// </summary>
        private readonly IProductDescriptionsRepository _repository;

        /// <summary>
        /// Initialize Vessel Product Descriptions handbook repository
        /// </summary>
        /// <param name="repository">Data repository</param>
        public VesselProductDescriptionDataProvider(IProductDescriptionsRepository repository) => _repository = repository;

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.VesselProductDescriptions;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (searchInfo.SearchByKey && !string.IsNullOrWhiteSpace(searchInfo.Search))
            {
                ProductDescriptionHandbookRecord record = _repository.Get(Convert.ToInt32(searchInfo.Search));
                var result = new List<LookupItem>();
                if (record != null)
                {
                    result.Add(new LookupItem { DisplayValue = record.Name, Value = record.Id });
                }

                return result;
            }

            IEnumerable<ProductDescriptionHandbookRecord> data = _repository.Search(searchInfo.Search, searchInfo.Limit,
                Convert.ToInt32(searchInfo.DependValue));
            return data.Select(x => new LookupItem { DisplayValue = x.Name, Value = x.Id });
        }
        /// <summary>
        /// Adds new value to handbook
        /// </summary>
        /// <param name="modelValue">Option value</param>
        /// <param name="dependValue">Additional data</param>
        public LookupItem Add(string modelValue, object dependValue)
        {
            var newRecord = new ProductDescriptionHandbookRecord { Name = modelValue, TariffId = Convert.ToInt32(dependValue) };
            _repository.Add(newRecord);
            _repository.Save();
            return new LookupItem { DisplayValue = newRecord.Name, Value = newRecord.Id };
        }
    }
}