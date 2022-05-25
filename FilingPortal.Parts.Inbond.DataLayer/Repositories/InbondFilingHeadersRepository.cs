using System.Collections.Generic;
using System.Data;
using System.Linq;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Parts.Inbond.DataLayer.Repositories
{
    /// <summary>
    /// Class for repository of <see cref="FilingHeader"/>
    /// </summary>
    public class InbondFilingHeadersRepository : BaseFilingHeadersRepository<FilingHeader>, IFilingHeaderRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InbondFilingHeadersRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public InbondFilingHeadersRepository(IUnitOfWorkFactory<UnitOfWorkInbondContext> unitOfWork) : base(unitOfWork) { }
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
            Set.Where(x => x.InbondRecords.Select(r => r.Id).Intersect(ids).Any());
        /// <summary>
        /// Gets the section by its name
        /// </summary>
        /// <param name="sectionName">The section name</param>
        protected override BaseSection GetSection(string sectionName)
        {
            var context = (InbondContext)UnitOfWork.Context;
            return context.Sections.FirstOrDefault(x => x.Name == sectionName);
        }
    }
}