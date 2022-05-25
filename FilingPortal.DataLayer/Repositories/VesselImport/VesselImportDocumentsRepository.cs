using System.Collections.Generic;
using System.Linq;
using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.VesselImport
{
    /// <summary>
    /// Class for repository of <see cref="VesselImportDocument"/>
    /// </summary>
    public class VesselImportDocumentsRepository : BaseDocumentRepository<VesselImportDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportDocumentsRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public VesselImportDocumentsRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Gets the list of documents by filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public override IEnumerable<VesselImportDocument> GetListByFilingHeader(int filingHeaderId)
        {
            var filingHeader = UnitOfWork.Context.Set<VesselImportFilingHeader>()?.FirstOrDefault(x => x.Id == filingHeaderId);
            if (filingHeader != null)
            {
                return filingHeader.Documents.Union(filingHeader.VesselInbounds.SelectMany(y => y.Documents));
            }
            return new List<VesselImportDocument>();
        }
    }
}