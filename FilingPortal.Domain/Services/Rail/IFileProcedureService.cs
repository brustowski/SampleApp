using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.Entities.Rail;
using Framework.Domain.Paging;
using System.Collections.Generic;

namespace FilingPortal.Domain.Services.Rail
{
    /// <summary>
    /// Interface describing File Procedure
    /// </summary>
    public interface IFileProcedureService: IFilingService<RailBdParsed>
    {
        /// <summary>
        /// Creates the initial filing header by specified bd parsed record ids
        /// </summary>
        /// <param name="inboundIds">The record ids</param>
        /// <param name="userAccount">User whose profile will be used for data mapping</param>
        OperationResultWithValue<int> CreateUnitTrainFilingHeader(IEnumerable<int> inboundIds, string userAccount = null);

        /// <summary>
        /// Creates the initial filing header by specified filter set and excluded records id 
        /// </summary>
        /// <param name="filtersSet">List of the filters <see cref="FiltersSet"/></param>
        /// <param name="userAccount">User whose profile will be used for data mapping</param>
        OperationResultWithValue<int> CreateUnitTrainFilingHeader(FiltersSet filtersSet, string userAccount = null);
    }
}