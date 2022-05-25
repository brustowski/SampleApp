using System.Collections.Generic;
using System.Linq;
using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.Rail
{
    /// <summary>
    /// Class for repository of <see cref="RailDocument"/>
    /// </summary>
    public class RailDocumentRepository : BaseDocumentRepository<RailDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailDocumentRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public RailDocumentRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Gets the list of documents by filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public override IEnumerable<RailDocument> GetListByFilingHeader(int filingHeaderId)
        {
            var filingHeader = UnitOfWork.Context.Set<RailFilingHeader>()?.FirstOrDefault(x => x.Id == filingHeaderId);
            if (filingHeader != null)
            {
                return filingHeader.RailDocuments.Union(filingHeader.RailBdParseds.SelectMany(y => y.Documents));
            }
            return new List<RailDocument>();
        }
    }
}