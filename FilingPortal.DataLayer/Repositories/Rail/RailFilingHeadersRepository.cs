using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories.Rail;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.DataLayer.Repositories.Rail
{
    /// <summary>
    /// Class for repository of <see cref="RailFilingHeader"/>
    /// </summary>
    public class RailFilingHeadersRepository : BaseFilingHeadersRepository<RailFilingHeader>, IRailFilingHeadersRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailFilingHeadersRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public RailFilingHeadersRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// The name of the create entry record stored procedure
        /// </summary>
        protected override string CreateEntryProcedureName => "sp_imp_rail_create_entry_records";

        /// <summary>
        /// The name of the delete entry record stored procedure
        /// </summary>
        protected override string DeleteEntryProcedureName => "sp_imp_rail_delete_entry_records";

        /// <summary>
        /// Finds the RailFilingHeaders by rail bd parsed identifiers
        /// </summary>
        /// <param name="bdParsedIds">The rail bd parsed identifiers</param>
        public override IEnumerable<RailFilingHeader> FindByInboundRecordIds(IEnumerable<int> bdParsedIds)
        {
            return Set.Where(x => x.RailBdParseds.Select(r => r.Id).Intersect(bdParsedIds).Any());
        }

        /// <summary>
        /// Gets the rail filing header with details by identifier
        /// </summary>
        /// <param name="id">The rail filing header identifier</param>
        public RailFilingHeader GetRailFilingHeaderWithDetails(int id)
        {
            return Set.Include("RailBdParseds.RailEdiMessage").FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Gets the section by its name
        /// </summary>
        /// <param name="sectionName">The section name</param>
        protected override BaseSection GetSection(string sectionName)
        {
            var context = (FilingPortalContext)UnitOfWork.Context;
            return context.RailSections.FirstOrDefault(x => x.Name == sectionName);
        }
    }
}