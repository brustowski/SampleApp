using System.Collections.Generic;
using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Parts.Common.Domain.Common.InboundTypes;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Framework.Domain;
using Framework.Domain.Paging;

namespace FilingPortal.Domain.Services
{
    public interface IAutofilingService<TAutofileEntity> : IFilingService<TAutofileEntity>
    where TAutofileEntity : Entity, IAutoFilingEntity
    {
        /// <summary>
        /// Update inbound records specified by ids.
        /// </summary>
        /// <param name="recordIds">A collection of the inbound record ids to update</param>
        /// <param name="user">The user who started the process</param>
        OperationResultWithValue<int[]> Update(IEnumerable<int> recordIds, AppUsersModel user);

        /// <summary>
        /// Update inbound records specified by filter set.
        /// </summary>
        /// <param name="filtersSet">List of the filters <see cref="FiltersSet"/></param>
        /// <param name="user">The user who started the process</param>
        OperationResultWithValue<int[]> Update(FiltersSet filtersSet, AppUsersModel user);

        /// <summary>
        /// Calls File procedure and set Autofiled flag
        /// </summary>
        /// <param name="filingHeaderIds">The filing identifiers</param>
        /// <param name="user">The user who started the process</param>
        void AutoFile(int[] filingHeaderIds, string user);
    }
}
