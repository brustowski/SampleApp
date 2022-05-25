using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.FieldConfigurations;
using Framework.Domain.Repositories;

namespace FilingPortal.Web.FieldConfigurations.TruckExport
{
    /// <summary>
    /// Defines truck export filing configuration factory
    /// </summary>
    public class TruckExportFilingConfigurationFactory : BaseFilingConfigurationFactory<TruckExportDefValuesManualReadModel, TruckExportDocument, TruckExportSection, TruckExportRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportFilingConfigurationFactory" /> class.
        /// </summary>
        /// <param name="fieldsRepository">The fields repository</param>
        /// <param name="documentsRepository">The documents repository</param>
        /// <param name="sectionsRepository">The sections repository</param>
        /// <param name="fieldBuilder">Field builder</param>
        /// <param name="inboundRecordsRepository">Inbound records repository</param>
        public TruckExportFilingConfigurationFactory(IDefValuesManualReadModelRepository<TruckExportDefValuesManualReadModel> fieldsRepository
            , IDocumentRepository<TruckExportDocument> documentsRepository
            , ISearchRepository<TruckExportSection> sectionsRepository
            , IInboundRecordFieldBuilder<TruckExportDefValuesManualReadModel> fieldBuilder
            , IInboundRecordsRepository<TruckExportRecord> inboundRecordsRepository) : base(fieldsRepository, documentsRepository, sectionsRepository, fieldBuilder, inboundRecordsRepository)
        {
            
        }

        /// <summary>
        /// Returns descendants of Truck Exports Section
        /// </summary>
        /// <param name="section">Truck export section</param>
        /// <returns></returns>
        protected override List<TruckExportSection> GetDescendants(TruckExportSection section) =>
            section.Descendants.ToList();
    }
}