using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Common;
using FilingPortal.Parts.Common.Domain.DTOs;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.PluginEngine.Common;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using FilingPortal.PluginEngine.Models.Fields;
using Framework.Domain.Repositories;
using Newtonsoft.Json;

namespace FilingPortal.PluginEngine.Services
{
    /// <summary>
    /// Provides the Filing parameters handlers for the Address parameter type
    /// </summary>
    class AddressFilingParametersHandler : IFilingParametersHandler
    {
        /// <summary>
        /// The overridden address repository
        /// </summary>
        private readonly ISearchRepository<AppAddress> _addressRepository;

        /// <summary>
        /// Initialize a new instance of the <see cref="AddressFilingParametersHandler"/> class
        /// </summary>
        /// <param name="addressRepository">The overridden address repository</param>
        public AddressFilingParametersHandler(ISearchRepository<AppAddress> addressRepository)
        {
            _addressRepository = addressRepository;
        }

        /// <summary>
        /// Executes additional actions on provided parameter
        /// </summary>
        /// <param name="parameter">The parameter to process</param>
        /// <param name="additional">List of the additional data</param>
        public void Process(InboundRecordParameter parameter, params object[] additional)
        {
            if (additional.Length == 0 || string.IsNullOrWhiteSpace(parameter.Value))
            {
                return;
            }

            BaseDefValuesManualReadModel defValue = additional.OfType<BaseDefValuesManualReadModel>().FirstOrDefault();

            if (defValue == null || defValue.ValueType != UIValueTypes.Address)
            {
                return;
            }

            try
            {
                AppAddress appAddress = parameter.Value.Map<string, AppAddress>();
                _addressRepository.AddOrUpdate(appAddress);
                _addressRepository.Save();
                parameter.Value = appAddress.Id.ToString();
            }
            catch
            {
                // ignored
            }
        }
    }
}
