using FilingPortal.Domain.Common;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories;
using Framework.Infrastructure;
using Framework.Infrastructure.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Common;
using FilingPortal.Parts.Common.Domain.DTOs;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Provides methods to work with filing parameters 
    /// </summary>
    /// <typeparam name="TDefValuesManual">Form configuration type</typeparam>
    /// <typeparam name="TDefValuesManualReadModel">Form configuration with value type</typeparam>
    internal class FilingParametersService<TDefValuesManual, TDefValuesManualReadModel> : IFilingParametersService<TDefValuesManual, TDefValuesManualReadModel>
        where TDefValuesManual : BaseDefValuesManual
        where TDefValuesManualReadModel : BaseDefValuesManualReadModel
    {
        /// <summary>
        /// Filing data fields configuration repository
        /// </summary>
        private readonly IDefValuesManualRepository<TDefValuesManual> _repository;

        /// <summary>
        /// Filing data fields with configuration repository 
        /// </summary>
        private readonly IDefValuesManualReadModelRepository<TDefValuesManualReadModel> _readModelRepository;

        /// <summary>
        /// Filing parameters handlers
        /// </summary>
        private readonly List<IFilingParametersHandler> _handlers;
       

        /// <summary>
        /// Initialize a new instance of the <see cref="FilingParametersService{TDefValuesManual,TDefValuesManualReadModel}"/> service.
        /// </summary>
        /// <param name="repository">Filing data fields configuration repository</param>
        /// <param name="readModelRepository">Filing data fields with configuration repository</param>
        /// <param name="handlers">Filing parameters handlers</param>
        public FilingParametersService(IDefValuesManualRepository<TDefValuesManual> repository
            , IDefValuesManualReadModelRepository<TDefValuesManualReadModel> readModelRepository
            , IEnumerable<IFilingParametersHandler> handlers)
        {
            _repository = repository;
            _readModelRepository = readModelRepository;
            _handlers = handlers.ToList();
        }

        /// <summary>
        /// Updates the filing parameters
        /// </summary>
        /// <param name="parameters">The filing parameters</param>
        public void UpdateFilingParameters(InboundRecordFilingParameters parameters)
        {
            try
            {
                InboundRecordFilingParameters ensuredParams = EnsureFilingParameters(parameters);
                _repository.UpdateValues(ensuredParams);
            }
            catch (Exception e)
            {
                AppLogger.Error(e, "FilingParametersService: Error occurred during updating Filing parameters process");
                throw;
            }
        }

        /// <summary>
        /// Recalculates values on form values change
        /// </summary>
        /// <param name="filingParameters">Current form values</param>
        public InboundRecordFilingParameters ProcessChanges(InboundRecordFilingParameters filingParameters)
        {
            try
            {
                return _repository.ProcessChanges(filingParameters);
            }
            catch (Exception e)
            {
                AppLogger.Error(e, "FilingParametersService: Error occurred during changes process");
                throw;
            }
        }

        /// <summary>
        /// Ensure that model contains all available data with latest updates
        /// </summary>
        /// <param name="model">The list of model to ensure</param>
        private InboundRecordFilingParameters EnsureFilingParameters(InboundRecordFilingParameters model)
        {
            var uiParams = model.Parameters.ToList();
            IEnumerable<TDefValuesManualReadModel> dbParams = _readModelRepository.GetDefValuesByFilingHeader(model.FilingHeaderId).ToList();

            foreach (TDefValuesManualReadModel dbParam in dbParams)
            {
                InboundRecordParameter uiParam = uiParams.FirstOrDefault(x => x.Id == dbParam.Id && x.RecordId == dbParam.RecordId);
                if (uiParam == null)
                {
                    uiParam = dbParam.Map<TDefValuesManualReadModel, InboundRecordParameter>();
                    uiParams.Add(uiParam);
                }

                if (!dbParam.Mandatory && string.IsNullOrWhiteSpace(uiParam.Value))
                {
                    uiParam.Value = null;
                }

                foreach (IFilingParametersHandler handler in _handlers)
                {
                    handler.Process(uiParam, dbParam);
                }
            }

            var result = new InboundRecordFilingParameters
            {
                FilingHeaderId = model.FilingHeaderId,
                Parameters = uiParams
            };

            InboundRecordFilingParameters calculatedParams = ProcessChanges(result);

            calculatedParams.Parameters.ForEach(par =>
            {
                InboundRecordParameter recordParameter = result.Parameters.First(x => x.Id == par.Id && x.RecordId == par.RecordId);
                recordParameter.Value = par.Value;
            });

            return result;
        }
    }
}