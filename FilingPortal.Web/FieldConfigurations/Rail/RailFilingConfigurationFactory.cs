using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.FieldConfigurations;
using Framework.Domain.Repositories;

namespace FilingPortal.Web.FieldConfigurations.Rail
{
    /// <summary>
    /// Defines rail export filing configuration factory
    /// </summary>
    public class RailFilingConfigurationFactory : BaseFilingConfigurationFactory<RailDefValuesManualReadModel, RailDocument, RailSection, RailBdParsed>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailFilingConfigurationFactory" /> class.
        /// </summary>
        /// <param name="fieldsRepository">The fields repository</param>
        /// <param name="documentsRepository">The documents repository</param>
        /// <param name="sectionsRepository">The sections repository</param>
        /// <param name="fieldBuilder">Field builder</param>
        /// <param name="inboundRecordsRepository">Inbound records repository</param>
        public RailFilingConfigurationFactory(IDefValuesManualReadModelRepository<RailDefValuesManualReadModel> fieldsRepository
            , IDocumentRepository<RailDocument> documentsRepository
            , ISearchRepository<RailSection> sectionsRepository
            , IInboundRecordFieldBuilder<RailDefValuesManualReadModel> fieldBuilder
            , IInboundRecordsRepository<RailBdParsed> inboundRecordsRepository) : base(fieldsRepository, documentsRepository, sectionsRepository, fieldBuilder, inboundRecordsRepository)
        {
        }

        /// <summary>
        /// Returns descendants of Rail section
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        protected override List<RailSection> GetDescendants(RailSection section) => section.Descendants.ToList();
    }
}