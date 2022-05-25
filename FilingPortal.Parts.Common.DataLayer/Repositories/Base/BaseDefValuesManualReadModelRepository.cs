using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Parts.Common.DataLayer.Repositories.Base
{
    /// <summary>
    /// Class for repository of <see cref="TDefValuesManualReadModel"/>
    /// </summary>
    public abstract class BaseDefValuesManualReadModelRepository<TDefValuesManualReadModel> : Repository<TDefValuesManualReadModel>, IDefValuesManualReadModelRepository<TDefValuesManualReadModel>
        where TDefValuesManualReadModel : BaseDefValuesManualReadModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDefValuesManualReadModelRepository{TDefValuesManualReadModel}"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        protected BaseDefValuesManualReadModelRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Gets predefined Additional Parameter fields by specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public IEnumerable<TDefValuesManualReadModel> GetAdditionalParametersByFilingHeader(int filingHeaderId)
        {
            return Set
                .Where(x => x.FilingHeaderId == filingHeaderId && x.Manual > 0)
                .OrderBy(x => x.Manual)
                .ToList();
        }

        /// <summary>
        /// Returns all filing header values by filing header ids
        /// </summary>
        /// <param name="filingHeaderIds">List of filing headers</param>
        public virtual IEnumerable<TDefValuesManualReadModel> GetAllDataByFilingHeaderIds(IEnumerable<int> filingHeaderIds)
        {
            return Set.Where(x => filingHeaderIds.Contains(x.FilingHeaderId))
                .ToList();
        }

        // todo: remove unnecessary method after all workflow migration 
        /// <summary>
        /// Gets all predefined fields by specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        /// <param name="operationId">Unique operation id</param>
        public virtual IEnumerable<TDefValuesManualReadModel> GetMappedValues(int filingHeaderId, Guid operationId)
        {
            return this.GetDefValuesByFilingHeader(filingHeaderId);
        }

        /// <summary>
        /// Gets predefined Common Data fields by specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public IEnumerable<TDefValuesManualReadModel> GetCommonDataByFilingHeader(int filingHeaderId)
        {
            return Set
                .Where(x => x.FilingHeaderId == filingHeaderId && x.Manual == 0 && x.DisplayOnUI > 0)
                .ToList();
        }

        /// <summary>
        /// Gets all predefined fields by specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public virtual IEnumerable<TDefValuesManualReadModel> GetDefValuesByFilingHeader(int filingHeaderId) =>
            Set
            .Where(x => x.FilingHeaderId == filingHeaderId)
            .ToList();

        /// <summary>
        /// Gets values for some columns in defValuesManual for displaying in single filing grid
        /// </summary>
        /// <param name="filingHeaderIds">List of filing headers</param>
        /// <param name="columnNames">List of column names</param>
        public IEnumerable<TDefValuesManualReadModel> GetSingleFilingData(IEnumerable<int> filingHeaderIds, IEnumerable<string> columnNames)
        {
            IEnumerable<string> columns = new List<string> {
                "FilingHeadersFk"
            }.Union(columnNames);

            return Set
                .Where(x =>
                   filingHeaderIds.Contains(x.FilingHeaderId)
                && columns.Contains(x.ColumnName)).AsEnumerable();
        }
        /// <summary>
        /// Count existing filing ids in repository
        /// </summary>
        /// <param name="filingHeaderIds">list of filing headers to check existence</param>
        public virtual int GetTotalMatches(IEnumerable<int> filingHeaderIds) =>
            Set
            .Where(x => filingHeaderIds.Contains(x.FilingHeaderId))
            .Select(x => x.FilingHeaderId)
            .Distinct()
            .Count();
    }
}