using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.FieldConfigurations;
using Framework.Domain.Repositories;

namespace FilingPortal.Web.FieldConfigurations.VesselExport
{
    /// <summary>
    /// Defines vessel export filing configuration factory
    /// </summary>
    public class VesselExportFilingConfigurationFactory : BaseFilingConfigurationFactory<VesselExportDefValuesManualReadModel, VesselExportDocument, VesselExportSection, VesselExportRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportFilingConfigurationFactory" /> class.
        /// </summary>
        /// <param name="fieldsRepository">The fields repository</param>
        /// <param name="documentsRepository">The documents repository</param>
        /// <param name="sectionsRepository">The sections repository</param>
        /// <param name="fieldBuilder">Field builder</param>
        /// <param name="inboundRecordsRepository">Inbound records repository</param>
        public VesselExportFilingConfigurationFactory(IDefValuesManualReadModelRepository<VesselExportDefValuesManualReadModel> fieldsRepository
            , IDocumentRepository<VesselExportDocument> documentsRepository
            , ISearchRepository<VesselExportSection> sectionsRepository
            , IInboundRecordFieldBuilder<VesselExportDefValuesManualReadModel> fieldBuilder,
            IInboundRecordsRepository<VesselExportRecord> inboundRecordsRepository) : base(fieldsRepository, documentsRepository, sectionsRepository, fieldBuilder, inboundRecordsRepository)
        {
            
        }

        /// <summary>
        /// Returns descendants of Vessel Exports Section
        /// </summary>
        /// <param name="section">Vessel export section</param>
        /// <returns></returns>
        protected override List<VesselExportSection> GetDescendants(VesselExportSection section) =>
            section.Descendants.ToList();
    }
}