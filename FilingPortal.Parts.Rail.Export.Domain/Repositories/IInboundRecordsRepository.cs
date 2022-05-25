﻿using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Rail.Export.Domain.Entities;

namespace FilingPortal.Parts.Rail.Export.Domain.Repositories
{
    /// <summary>
    /// Interface for repository of <see cref="InboundRecord"/>
    /// </summary>
    public interface IInboundRecordsRepository : IInboundRecordsRepository<InboundRecord>
    {
        /// <summary>
        /// Checks if a record is present in the database
        /// </summary>
        /// <param name="record">The record to check</param>
        bool IsDuplicated(InboundRecord record);
    }
}
