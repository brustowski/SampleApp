using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.PluginEngine.DataProviders.Cargowise
{
    /// <summary>
    /// Provider for Cargowise FIRMs codes data 
    /// </summary>
    public class CargowiseFirmsCodesDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Cargowise FIRMs Codes repository
        /// </summary>
        private readonly IFirmsCodesRepository _repository;

        /// <summary>
        /// Initialize Vessel Discharge Terminal handbook repository
        /// </summary>
        /// <param name="repository">Data repository</param>
        public CargowiseFirmsCodesDataProvider(IFirmsCodesRepository repository) => _repository = repository;

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.CargowiseFirmsCodes;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (searchInfo.SearchByKey)
            {
                CargowiseFirmsCodes record = _repository.Get(Convert.ToInt32(searchInfo.Search));
                var result = new List<LookupItem>();
                if (record != null)
                {
                    result.Add(new LookupItem { DisplayValue = $"{record.FirmsCode} - {record.Name}", Value = record.Id });
                }

                return result;
            }

            IEnumerable<CargowiseFirmsCodes> data;
            if (!string.IsNullOrEmpty(searchInfo.DependOn))
                data = _repository.Search(searchInfo.Search, searchInfo.Limit,
                    searchInfo.DependValue != null ? Convert.ToInt32(searchInfo.DependValue) : (int?) null);
            else
                data = _repository.Search(searchInfo.Search, searchInfo.Limit);

            return data.Select(x => new LookupItem { DisplayValue = $"{x.FirmsCode} - {x.Name}", Value = x.Id });
        }
    }
}