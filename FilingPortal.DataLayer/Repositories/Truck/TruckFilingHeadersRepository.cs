using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Repositories.Truck;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.DataLayer.Repositories.Truck
{
    /// <summary>
    /// Represents the repository of the <see cref="TruckFilingHeader"/>
    /// </summary>
    public class TruckFilingHeadersRepository : BaseFilingHeadersRepository<TruckFilingHeader>, ITruckFilingHeadersRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckFilingHeadersRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public TruckFilingHeadersRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// The name of the create entry record stored procedure
        /// </summary>
        protected override string CreateEntryProcedureName => "sp_imp_truck_create_entry_records";

        /// <summary>
        /// The name of the delete entry record stored procedure
        /// </summary>
        protected override string DeleteEntryProcedureName => "sp_imp_truck_delete_entry_records";

        /// <summary>
        /// Finds the Truck Filing Headers by the identifiers
        /// </summary>
        /// <param name="ids">The identifiers</param>
        public override IEnumerable<TruckFilingHeader> FindByInboundRecordIds(IEnumerable<int> ids)
        {
            return Set.Where(x => x.TruckInbounds.Select(r => r.Id).Intersect(ids).Any());
        }

        /// <summary>
        /// Gets the truck filing header with details by identifier
        /// </summary>
        /// <param name="id">The truck filing header identifier</param>
        public TruckFilingHeader GetTruckFilingHeaderWithDetails(int id)
        {
            return Set.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Gets the section by its name
        /// </summary>
        /// <param name="sectionName">The section name</param>
        protected override BaseSection GetSection(string sectionName)
        {
            var context = (FilingPortalContext)UnitOfWork.Context;
            return context.TruckSections.FirstOrDefault(x => x.Name == sectionName);
        }
    }
}