using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories
{
    public interface IFilingDataRepository<TFilingData>: ISearchRepository<TFilingData>
        where TFilingData: BaseFilingData
    {
        /// <summary>
        /// Gets unique data for selected filing headers
        /// </summary>
        /// <param name="ids">Filing Headers ids</param>
        IList<TFilingData> GetByFilingNumbers(params int[] ids);
    }
}
