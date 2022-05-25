using System.Collections.Generic;
using System.Linq;
using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.TruckExport
{
    /// <summary>
    /// Class for repository of <see cref="TruckExportDocument"/>
    /// </summary>
    public class TruckExportDocumentsRepository : BaseDocumentRepository<TruckExportDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportDocumentsRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public TruckExportDocumentsRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Gets the list of documents by filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public override IEnumerable<TruckExportDocument> GetListByFilingHeader(int filingHeaderId)
        {
            var filingHeader = UnitOfWork.Context.Set<TruckExportFilingHeader>()?.FirstOrDefault(x => x.Id == filingHeaderId);
            if (filingHeader != null)
            {
                return filingHeader.Documents.Union(filingHeader.TruckExports.SelectMany(y => y.Documents));
            }
            return new List<TruckExportDocument>();
        }
    }
}