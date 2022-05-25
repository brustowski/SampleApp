using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Repositories;
using Framework.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.FieldConfigurations;

namespace FilingPortal.Web.FieldConfigurations.Vessel
{
    /// <summary>
    /// Defines Vessel Filing Configuration factory
    /// </summary>
    public class VesselFilingConfigurationFactory : BaseFilingConfigurationFactory<VesselImportDefValuesManualReadModel, VesselImportDocument, VesselImportSection, VesselImportRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselFilingConfigurationFactory" /> class.
        /// </summary>
        /// <param name="fieldsRepository">The fields repository</param>
        /// <param name="documentsRepository">The documents repository</param>
        /// <param name="sectionsRepository">The sections repository</param>
        /// <param name="fieldBuilder">Field builder</param>
        /// <param name="inboundRecordsRepository">inbound records repository</param>
        public VesselFilingConfigurationFactory(IDefValuesManualReadModelRepository<VesselImportDefValuesManualReadModel> fieldsRepository
            , IDocumentRepository<VesselImportDocument> documentsRepository
            , ISearchRepository<VesselImportSection> sectionsRepository
            , IInboundRecordFieldBuilder<VesselImportDefValuesManualReadModel> fieldBuilder
            , IInboundRecordsRepository<VesselImportRecord> inboundRecordsRepository) : base(fieldsRepository, documentsRepository, sectionsRepository, fieldBuilder, inboundRecordsRepository)
        {
        }

        /// <summary>
        /// Returns descendants of current section
        /// </summary>
        /// <param name="section">Vessel section</param>
        protected override List<VesselImportSection> GetDescendants(VesselImportSection section) =>
            section.Descendants.ToList();
    }
}