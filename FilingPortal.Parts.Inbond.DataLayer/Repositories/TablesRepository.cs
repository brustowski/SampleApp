﻿using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Inbond.Domain.Entities;
using Framework.DataLayer;

namespace FilingPortal.Parts.Inbond.DataLayer.Repositories
{
    /// <summary>
    /// Class for repository of <see cref="Tables"/>
    /// </summary>
    public class TablesRepository : BaseTablesRepository<Tables>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TablesRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public TablesRepository(IUnitOfWorkFactory<UnitOfWorkInbondContext> unitOfWork) : base(unitOfWork)
        { }
    }
}
