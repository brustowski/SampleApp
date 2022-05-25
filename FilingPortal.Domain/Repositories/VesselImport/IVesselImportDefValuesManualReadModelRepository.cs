﻿using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.VesselImport
{
    /// <summary>
    /// Interface for repository of <see cref="VesselImportDefValuesManualReadModel"/>
    /// </summary>
    public interface IVesselImportDefValuesManualReadModelRepository : IDefValuesManualReadModelRepository<VesselImportDefValuesManualReadModel>
    { }
}