using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.FieldConfigurations;
using Framework.Domain.Repositories;

namespace FilingPortal.Web.FieldConfigurations.Pipeline
{
    /// <summary>
    /// Defines truck export filing configuration factory
    /// </summary>
    public class PipelineFilingConfigurationFactory : BaseFilingConfigurationFactory<PipelineDefValueManualReadModel, PipelineDocument, PipelineSection, PipelineInbound>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineFilingConfigurationFactory" /> class.
        /// </summary>
        /// <param name="fieldsRepository">The fields repository</param>
        /// <param name="documentsRepository">The documents repository</param>
        /// <param name="sectionsRepository">The sections repository</param>
        /// <param name="fieldBuilder">Field builder</param>
        /// <param name="inboundRecordsRepository">Inbound records repository</param>
        public PipelineFilingConfigurationFactory(IDefValuesManualReadModelRepository<PipelineDefValueManualReadModel> fieldsRepository
            , IDocumentRepository<PipelineDocument> documentsRepository
            , ISearchRepository<PipelineSection> sectionsRepository
            , IInboundRecordFieldBuilder<PipelineDefValueManualReadModel> fieldBuilder,
            IInboundRecordsRepository<PipelineInbound> inboundRecordsRepository) : base(fieldsRepository, documentsRepository, sectionsRepository, fieldBuilder, inboundRecordsRepository)
        {
        }

        /// <summary>
        /// Returns descendants of Pipeline section
        /// </summary>
        /// <param name="section">Pipeline section</param>
        protected override List<PipelineSection> GetDescendants(PipelineSection section) =>
            section.Descendants.ToList();
    }
}