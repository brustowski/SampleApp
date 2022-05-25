using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Repositories;
using Framework.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.FieldConfigurations;

namespace FilingPortal.Web.FieldConfigurations.Truck
{
    /// <summary>
    /// Defines Truck Filing Configuration factory
    /// </summary>
    public class TruckFilingConfigurationFactory : BaseFilingConfigurationFactory<TruckDefValueManualReadModel, TruckDocument, TruckSection, TruckInbound>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckFilingConfigurationFactory" /> class.
        /// </summary>
        /// <param name="fieldsRepository">The fields repository</param>
        /// <param name="documentsRepository">The documents repository</param>
        /// <param name="sectionsRepository">The sections repository</param>
        /// <param name="fieldBuilder">Field builder</param>
        /// <param name="inboundRecordsRepository">Inbound records repository</param>
        public TruckFilingConfigurationFactory(IDefValuesManualReadModelRepository<TruckDefValueManualReadModel> fieldsRepository
            , IDocumentRepository<TruckDocument> documentsRepository
            , ISearchRepository<TruckSection> sectionsRepository
            , IInboundRecordFieldBuilder<TruckDefValueManualReadModel> fieldBuilder
            , IInboundRecordsRepository<TruckInbound> inboundRecordsRepository) : base(fieldsRepository, documentsRepository, sectionsRepository, fieldBuilder, inboundRecordsRepository)
        {
        }

        /// <summary>
        /// Returns descendants of current section
        /// </summary>
        /// <param name="section">Truck section</param>
        protected override List<TruckSection> GetDescendants(TruckSection section) =>
            section.Descendants.ToList();
    }
}