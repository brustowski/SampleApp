using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Validators;
using FilingPortal.Domain.Validators.Common;
using FilingPortal.Parts.Common.Domain.AgileSettings;
using FilingPortal.Parts.Common.Domain.Common;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.Domain;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Service that prepares SingleFilingGrid
    /// </summary>
    public class SingleFilingGridWorker<TDefValuesReadModel, TDefValuesManualReadModel, TDocument>
        : ISingleFilingGridWorker<TDefValuesReadModel, TDefValuesManualReadModel, TDocument>
            where TDefValuesReadModel : Entity
            where TDefValuesManualReadModel : BaseDefValuesManualReadModel
            where TDocument : BaseDocument
    {
        /// <summary>
        /// Grid fields configuration repository
        /// </summary>
        readonly IAgileConfiguration<TDefValuesReadModel> _agileGridRepository;
        /// <summary>
        /// Form configuration repository
        /// </summary>
        readonly IDefValuesManualReadModelRepository<TDefValuesManualReadModel> _repository;
        /// <summary>
        /// Documents repository
        /// </summary>
        readonly IDocumentRepository<TDocument> _documentsRepository;
        /// <summary>
        /// Model validator
        /// </summary>
        protected readonly IDefValuesManualValidator<TDefValuesManualReadModel> Validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleFilingGridWorker{TDefValuesReadModel,TDefValuesManualReadModel,TDocument}" /> class
        /// </summary>
        /// <param name="repository">Form configuration repository</param>
        /// <param name="agileGridRepository">Grid fields configuration repository</param>
        /// <param name="documentsRepository">Documents repository</param>
        /// <param name="validator">Model validator</param>
        public SingleFilingGridWorker(IDefValuesManualReadModelRepository<TDefValuesManualReadModel> repository,
            IAgileConfiguration<TDefValuesReadModel> agileGridRepository,
            IDocumentRepository<TDocument> documentsRepository,
            IDefValuesManualValidator<TDefValuesManualReadModel> validator)
        {
            _repository = repository;
            _agileGridRepository = agileGridRepository;
            _documentsRepository = documentsRepository;
            Validator = validator;
        }

        /// <summary>
        /// Returns page with all required data based on dynamic configuration
        /// </summary>
        /// <param name="filingHeaderIds">The filing header ids</param>
        public IDictionary<int, FPDynObject> GetData(IEnumerable<int> filingHeaderIds)
        {
            var idsList = filingHeaderIds?.ToList() ?? new List<int>();

            IEnumerable<AgileField> fields = _agileGridRepository.GetFields();

            var pagedResult = _repository.GetAllDataByFilingHeaderIds(idsList);

            IDictionary<int, int> documentsAmount = _documentsRepository.GetDocumentsAmount(idsList);

            return ConvertModelToDynamic(pagedResult, documentsAmount, fields.Select(x => x.FieldId));
        }

        /// <summary>
        /// Returns amount of rows to return
        /// </summary>
        /// <param name="values">The values<see cref="IEnumerable{int}"/></param>
        public int GetTotalMatches(IEnumerable<int> values) => _repository.GetTotalMatches(values);

        /// <summary>
        /// Converts values from repository to list of models
        /// </summary>
        /// <param name="model">Raw data from repository</param>
        /// <param name="documentsAmount">The documents amount dictionary</param>
        /// <param name="includeFields">Fields in model</param>
        private IDictionary<int, FPDynObject> ConvertModelToDynamic(IEnumerable<TDefValuesManualReadModel> model, IDictionary<int, int> documentsAmount, IEnumerable<DefValuesUniqueData> includeFields)
        {
            var result = new Dictionary<int, FPDynObject>();

            var groups = model.GroupBy(x => x.FilingHeaderId).OrderBy(x => x.Key).ToList();

            foreach (var g in groups)
            {
                var fields = g.Where(x => includeFields.Contains(x.GetUniqueData()))
                    .DistinctBy(x => new {x.RecordId, uniqueData = x.GetUniqueData()});

                var properties = new Dictionary<string, object>();
                foreach (var field in fields)
                {
                    if (properties.ContainsKey(field.ColumnName))
                    {
                        properties[field.ColumnName] = $"{properties[field.ColumnName]}; {field.Value}";
                    }
                    else
                    {
                        properties.Add(field.ColumnName, field.Value);
                    }
                }
                properties.Add("FilingHeaderId", g.Key);
                properties.Add("DocsAmount", documentsAmount.FirstOrDefault(x => x.Key == g.Key).Value);

                var validationResults = GetValidationResults(g);

                properties.Add("Errors", validationResults.Select(x => new { FieldId = x.Key.Id, x.Value.Errors }));
                var dynObject = new FPDynObject(properties);

                result.Add(g.Key, dynObject);
            }

            return result;
        }

        /// <summary>
        /// Validates values and return validation results for records
        /// </summary>
        /// <param name="grouping">Form values, grouped by filing header</param>
        protected virtual IEnumerable<KeyValuePair<TDefValuesManualReadModel, DetailedValidationResult>> GetValidationResults(IGrouping<int, TDefValuesManualReadModel> grouping)
        {
            return Validator.ValidateDatabaseModels(grouping.ToArray())
                .Where(x => !x.Value.IsValid);
        }
    }
}
