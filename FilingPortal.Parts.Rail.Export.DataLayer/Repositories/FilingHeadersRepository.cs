using System.Collections.Generic;
using System.Linq;
using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Rail.Export.Domain.Entities;
using FilingPortal.Parts.Rail.Export.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Parts.Rail.Export.DataLayer.Repositories
{
    /// <summary>
    /// Class for repository of <see cref="FilingHeader"/>
    /// </summary>
    public class FilingHeadersRepository : BaseFilingHeadersRepository<FilingHeader>, IFilingHeadersRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilingHeadersRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public FilingHeadersRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork) { }
        /// <summary>
        /// The name of the create entry record stored procedure
        /// </summary>
        protected override string CreateEntryProcedureName => "sp_create_entry_records";
        /// <summary>
        /// The name of the delete entry record stored procedure
        /// </summary>
        protected override string DeleteEntryProcedureName => "sp_delete_entry_records";
        /// <summary>
        /// Finds the Filing Headers by inbound record identifiers
        /// </summary>
        /// <param name="ids">The inbound records identifiers</param>
        /// <returns>The <see cref="IEnumerable{FilingHeader}"/></returns>
        public override IEnumerable<FilingHeader> FindByInboundRecordIds(IEnumerable<int> ids) =>
            Set.Where(x => x.InboundRecords.Select(r => r.Id).Intersect(ids).Any());

        /// <summary>
        /// Tries to set new confirmation status and returns correct confirmation result
        /// </summary>
        /// <param name="filingHeaderId">Filing header id</param>
        /// <param name="confirmed">Confirmation status</param>
        public bool UpdateConfirmation(int filingHeaderId, bool confirmed)
        {
            FilingHeader filingHeader = Get(filingHeaderId);
            filingHeader.Confirmed = confirmed;
            Save();
            return filingHeader.Confirmed;
        }

        /// <summary>
        /// Gets the section by its name
        /// </summary>
        /// <param name="sectionName">The section name</param>
        protected override BaseSection GetSection(string sectionName)
        {
            var context = (PluginContext)UnitOfWork.Context;
            return context.Set<Section>().FirstOrDefault(x => x.Name == sectionName);
        }
    }
}