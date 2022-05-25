using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities;
using Framework.Domain;
using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Services
{
    public interface ISingleFilingGridWorker<TDefValuesReadModel, TDefValuesManualReadModel, TDocument>
        where TDefValuesReadModel : Entity
        where TDefValuesManualReadModel : BaseDefValuesManualReadModel
        where TDocument : BaseDocument
    {
        /// <summary>
        /// Gets single filing grid information
        /// </summary>
        /// <param name="filingHeaderIds">filing header ids</param>
        IDictionary<int, FPDynObject> GetData(IEnumerable<int> filingHeaderIds);

        /// <summary>
        /// Gets single filing grid records amount
        /// </summary>
        /// <param name="values">filing header ids</param>
        int GetTotalMatches(IEnumerable<int> values);
    }
}