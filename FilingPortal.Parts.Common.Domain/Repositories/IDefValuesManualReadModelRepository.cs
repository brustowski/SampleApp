using System;
using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Common.Domain.Repositories
{
    /// <summary>
    /// Interface for repository of <see cref="TDefValuesManualReadModel"/>
    /// </summary>
    public interface IDefValuesManualReadModelRepository<TDefValuesManualReadModel> : IRepository<TDefValuesManualReadModel>
        where TDefValuesManualReadModel: BaseDefValuesManualReadModel
    {
        /// <summary>
        /// Gets predefined DefValues by specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        IEnumerable<TDefValuesManualReadModel> GetAdditionalParametersByFilingHeader(int filingHeaderId);

        /// <summary>
        /// Gets predefined Common Data fields by specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        IEnumerable<TDefValuesManualReadModel> GetCommonDataByFilingHeader(int filingHeaderId);

        /// <summary>
        /// Gets all predefined fields by specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        IEnumerable<TDefValuesManualReadModel> GetDefValuesByFilingHeader(int filingHeaderId);

        /// <summary>
        /// Gets values for some columns in defValuesManual for displaying in single filing grid
        /// </summary>
        /// <param name="filingHeaderId">list of filing headers</param>
        /// <param name="columnNames">list of column names</param>
        IEnumerable<TDefValuesManualReadModel> GetSingleFilingData(IEnumerable<int> filingHeaderId, IEnumerable<string> columnNames);

        /// <summary>
        /// Count existing filing ids in repository
        /// </summary>
        /// <param name="filingHeaderIds">list of filing headers to check existence</param>
        int GetTotalMatches(IEnumerable<int> filingHeaderIds);

        /// <summary>
        /// Returns all filing header values by filing header ids
        /// </summary>
        /// <param name="filingHeaderIds">List of filing headers</param>
        IEnumerable<TDefValuesManualReadModel> GetAllDataByFilingHeaderIds(IEnumerable<int> filingHeaderIds);

        /// <summary>
        /// Gets all predefined fields by specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        /// <param name="operationId">Unique operation id</param>
        IEnumerable<TDefValuesManualReadModel> GetMappedValues(int filingHeaderId, Guid operationId);
    }
}