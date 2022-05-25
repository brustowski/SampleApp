using System.Collections.Generic;
using System.Linq;
using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.VesselExport
{
    /// <summary>
    /// Class for repository of <see cref="VesselExportDocument"/>
    /// </summary>
    public class VesselExportDocumentsRepository : BaseDocumentRepository<VesselExportDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportDocumentsRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public VesselExportDocumentsRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Gets the list of documents by filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public override IEnumerable<VesselExportDocument> GetListByFilingHeader(int filingHeaderId)
        {
            var filingHeader = UnitOfWork.Context.Set<VesselExportFilingHeader>()?.FirstOrDefault(x => x.Id == filingHeaderId);
            if (filingHeader != null)
            {
                return filingHeader.Documents.Union(filingHeader.VesselExports.SelectMany(y => y.Documents));
            }
            return new List<VesselExportDocument>();
        }
    }
}