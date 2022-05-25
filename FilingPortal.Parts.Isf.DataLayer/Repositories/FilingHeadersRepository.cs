﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Isf.Domain.Entities;
using FilingPortal.Parts.Isf.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Parts.Isf.DataLayer.Repositories
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
        /// Gets the section by its name
        /// </summary>
        /// <param name="sectionName">The section name</param>
        protected override BaseSection GetSection(string sectionName)
        {
            var context = (PluginContext)UnitOfWork.Context;
            return context.Sections.FirstOrDefault(x => x.Name == sectionName);
        }
    }
}