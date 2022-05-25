﻿using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using Framework.DataLayer;

namespace FilingPortal.Parts.Zones.Ftz214.DataLayer.Repositories
{
    /// <summary>
    /// Class for repository of the <see cref="DefValue"/>
    /// </summary>
    public class DefValuesRepository : BaseDefValuesRepository<DefValue, Section>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefValuesRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public DefValuesRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork) { }
    }
}
