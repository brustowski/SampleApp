using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Validators
{
    /// <summary>
    /// Validator for selected inbound records
    /// </summary>
    public class InboundListValidator : BaseListInboundValidator<InboundReadModel, FilingHeader>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseListInboundValidator{TModel,TFilingHeader}" /> class
        /// </summary>
        /// <param name="readModelRepository">The read model repository</param>
        /// <param name="filingHeadersRepository">The filing header repository</param>
        public InboundListValidator(ISearchRepository<InboundReadModel> readModelRepository, IFilingHeaderRepository<FilingHeader> filingHeadersRepository) : base(readModelRepository, filingHeadersRepository)
        {
        }

        protected override IEnumerable<int> GetInboundRecordsIds(FilingHeader filingHeader)
        {
            return filingHeader.InboundRecords.Select(x => x.Id);
        }
    }
}
