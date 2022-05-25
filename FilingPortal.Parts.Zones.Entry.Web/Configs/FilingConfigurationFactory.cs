using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using FilingPortal.PluginEngine.FieldConfigurations;
using Framework.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Parts.Zones.Entry.Web.Configs
{
    /// <summary>
    /// Defines filing configuration factory
    /// </summary>
    public class FilingConfigurationFactory : BaseFilingConfigurationFactory<DefValueManualReadModel, Document, Section, InboundRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilingConfigurationFactory" /> class.
        /// </summary>
        /// <param name="fieldsRepository">The fields repository</param>
        /// <param name="documentsRepository">The documents repository</param>
        /// <param name="sectionsRepository">The sections repository</param>
        /// <param name="fieldBuilder">Field builder</param>
        /// <param name="inboundRecordsRepository">Inbound records repository</param>
        public FilingConfigurationFactory(IDefValuesManualReadModelRepository<DefValueManualReadModel> fieldsRepository
            , IDocumentRepository<Document> documentsRepository
            , ISearchRepository<Section> sectionsRepository
            , IInboundRecordFieldBuilder<DefValueManualReadModel> fieldBuilder,
            IInboundRecordsRepository<InboundRecord> inboundRecordsRepository) : base(fieldsRepository, documentsRepository, sectionsRepository, fieldBuilder, inboundRecordsRepository)
        {

        }

        /// <summary>
        /// Returns descendants of Section
        /// </summary>
        /// <param name="section">Section</param>
        /// <returns></returns>
        protected override List<Section> GetDescendants(Section section) =>
            section.Descendants.ToList();
    }
}