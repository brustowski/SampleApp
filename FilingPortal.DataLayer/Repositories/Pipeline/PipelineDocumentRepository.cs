using System.Collections.Generic;
using System.Linq;
using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.Pipeline
{
    /// <summary>
    /// Represents the repository of the <see cref="PipelineDocument"/>
    /// </summary>
    public class PipelineDocumentRepository : BaseDocumentRepository<PipelineDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineDocumentRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public PipelineDocumentRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Gets the list of documents by filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public override IEnumerable<PipelineDocument> GetListByFilingHeader(int filingHeaderId)
        {
            var filingHeader = UnitOfWork.Context.Set<PipelineFilingHeader>()?.FirstOrDefault(x => x.Id == filingHeaderId);
            if (filingHeader != null)
            {
                return filingHeader.PipelineDocuments.Union(filingHeader.PipelineInbounds.SelectMany(y => y.Documents));
            }
            return new List<PipelineDocument>();
        }
    }
}