using FilingPortal.Parts.Common.Domain.Common.InboundTypes;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using Framework.Domain;
using Framework.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.PluginEngine.FieldConfigurations
{
    /// <summary>
    /// Abstract factory that builds fields configuration
    /// </summary>
    /// <typeparam name="TDefValuesManualReadModel">Field model</typeparam>
    /// <typeparam name="TDocument">Document model</typeparam>
    /// <typeparam name="TSection">Section model</typeparam>
    /// <typeparam name="TInboundType">Inbound record type</typeparam>
    public abstract class BaseFilingConfigurationFactory<TDefValuesManualReadModel, TDocument, TSection, TInboundType> : IFilingConfigurationFactory<TDefValuesManualReadModel>
        where TDefValuesManualReadModel : BaseDefValuesManualReadModel
        where TDocument : BaseDocument
        where TSection : BaseSection
        where TInboundType : Entity, IInboundType
    {
        /// <summary>
        /// The fields repository
        /// </summary>
        private readonly IDefValuesManualReadModelRepository<TDefValuesManualReadModel> _fieldsRepository;

        /// <summary>
        /// The truck document repository
        /// </summary>
        private readonly IDocumentRepository<TDocument> _documentsRepository;
        /// <summary>
        /// The sections repository
        /// </summary>
        private readonly ISearchRepository<TSection> _sectionsRepository;

        /// <summary>
        /// The field builder
        /// </summary>
        private readonly IInboundRecordFieldBuilder<TDefValuesManualReadModel> _fieldBuilder;

        /// <summary>
        /// Filing headers repository
        /// </summary>
        private readonly IInboundRecordsRepository<TInboundType> _inboundRecordsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFilingConfigurationFactory{TDefValuesManualReadModel, TDocument, TSection, TFilingHeader}" /> class.
        /// </summary>
        /// <param name="fieldsRepository">The fields repository</param>
        /// <param name="documentsRepository">The documents repository</param>
        /// <param name="sectionsRepository">The sections repository</param>
        /// <param name="fieldBuilder">Field builder</param>
        /// <param name="inboundRecordsRepository">Inbound records repository</param>
        protected BaseFilingConfigurationFactory(IDefValuesManualReadModelRepository<TDefValuesManualReadModel> fieldsRepository
            , IDocumentRepository<TDocument> documentsRepository
            , ISearchRepository<TSection> sectionsRepository
            , IInboundRecordFieldBuilder<TDefValuesManualReadModel> fieldBuilder
            , IInboundRecordsRepository<TInboundType> inboundRecordsRepository)
        {
            _fieldsRepository = fieldsRepository;
            _documentsRepository = documentsRepository;
            _sectionsRepository = sectionsRepository;
            _fieldBuilder = fieldBuilder;
            _inboundRecordsRepository = inboundRecordsRepository;
        }

        /// <summary>
        /// Create Filing configuration for specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">Filing header identifier</param>
        public FilingConfiguration CreateConfiguration(int filingHeaderId)
        {
            var configuration = new FilingConfiguration
            {
                FilingHeaderId = filingHeaderId,
                Fields = new List<FilingConfigurationField>()
            };

            var fields = _fieldsRepository.GetDefValuesByFilingHeader(filingHeaderId).ToList();
            foreach (TDefValuesManualReadModel field in fields)
            {
                FilingConfigurationField mappedField = field.Map<TDefValuesManualReadModel, FilingConfigurationField>();
                mappedField.Field = _fieldBuilder.CreateFrom(field, fields);
                if (mappedField.Field != null)
                {
                    configuration.Fields.Add(mappedField);
                }
            }

            IEnumerable<int> inboundRecords = _inboundRecordsRepository.GetByFilingId(filingHeaderId).Select(x => x.Id).ToList();

            IEnumerable<TDocument> documents = _documentsRepository.GetListByFilingHeader(filingHeaderId, inboundRecords);
            configuration.Documents = documents.Map<TDocument, InboundRecordDocumentViewModel>().ToList();

            IEnumerable<TSection> sections = _sectionsRepository.GetAll().Where(x => !x.IsHidden);
            configuration.Sections = sections.Map<TSection, FilingConfigurationSection>().ToList();

            return configuration;
        }

        /// <summary>
        /// Create Filing configuration for specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">Filing header identifier</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="recordId">Record Id</param>
        public FilingConfiguration CreateConfigurationForSection(int filingHeaderId, string sectionName, int recordId)
        {
            var sections = _sectionsRepository.GetAll().ToList();
            TSection section = sections.First(x => x.Name.Equals(sectionName));

            var fields = _fieldsRepository.GetDefValuesByFilingHeader(filingHeaderId).ToList();

            var result = ExtractSectionFields(recordId, fields, section).ToList();

            var configuration = new FilingConfiguration
            {
                FilingHeaderId = filingHeaderId,
                Fields = new List<FilingConfigurationField>(),
                Sections = sections.Map<TSection, FilingConfigurationSection>().ToList()
            };

            foreach (TDefValuesManualReadModel field in result)
            {
                FilingConfigurationField mappedField = field.Map<TDefValuesManualReadModel, FilingConfigurationField>();
                mappedField.Field = _fieldBuilder.CreateFrom(field, result);
                if (mappedField.Field != null)
                {
                    configuration.Fields.Add(mappedField);
                }
            }

            return configuration;
        }

        /// <summary>
        /// Create Filing configuration for specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">Filing header identifier</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="operationId">Unique operation id</param>
        public FilingConfiguration CreateConfigurationForSection(int filingHeaderId, string sectionName, Guid operationId)
        {
            var sections = _sectionsRepository.GetAll().ToList();

            var fields = _fieldsRepository.GetMappedValues(filingHeaderId, operationId).ToList();

            var configuration = new FilingConfiguration
            {
                FilingHeaderId = filingHeaderId,
                Fields = new List<FilingConfigurationField>(),
                Sections = sections.Map<TSection, FilingConfigurationSection>().ToList()
            };

            foreach (TDefValuesManualReadModel field in fields)
            {
                FilingConfigurationField mappedField = field.Map<TDefValuesManualReadModel, FilingConfigurationField>();
                mappedField.Field = _fieldBuilder.CreateFrom(field, fields);
                if (mappedField.Field != null)
                {
                    configuration.Fields.Add(mappedField);
                }
            }

            return configuration;
        }

        /// <summary>
        /// Returns descendants of current section
        /// </summary>
        /// <param name="section">Fields section</param>
        protected abstract List<TSection> GetDescendants(TSection section);

        /// <summary>
        /// Extracts fields belonging to the specified section and its descendants
        /// </summary>
        /// <param name="recordId">Record id</param>
        /// <param name="fields">Fields set</param>
        /// <param name="section">Fields section</param>
        protected virtual IEnumerable<TDefValuesManualReadModel> ExtractSectionFields(int recordId,
            List<TDefValuesManualReadModel> fields, TSection section)
        {
            var result = new List<TDefValuesManualReadModel>(fields.Where(x =>
                x.SectionName.Equals(section.Name) && x.RecordId == recordId));

            var stack = new Stack<(TSection Section, int ParentRecordId)>();
            foreach (TSection descendant in GetDescendants(section))
            {
                stack.Push((descendant, recordId));
            }

            while (stack.Count > 0)
            {
                (TSection railSection, var parentRecordId) = stack.Pop();
                var f = fields.Where(x =>
                    x.SectionName.Equals(railSection.Name) && x.ParentRecordId == parentRecordId).ToList();
                if (f.Count <= 0)
                {
                    continue;
                }

                result.AddRange(f);
                foreach (TSection descendant in GetDescendants(railSection))
                {
                    stack.Push((descendant, f[0].RecordId));
                }
            }

            return result;
        }
    }
}