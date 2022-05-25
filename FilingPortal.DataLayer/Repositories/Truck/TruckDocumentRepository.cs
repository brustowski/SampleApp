using System.Collections.Generic;
using System.Linq;
using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.Truck
{
    /// <summary>
    /// Class for repository of <see cref="TruckDocument"/>
    /// </summary>
    public class TruckDocumentRepository : BaseDocumentRepository<TruckDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckDocumentRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public TruckDocumentRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Gets the list of documents by filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public override IEnumerable<TruckDocument> GetListByFilingHeader(int filingHeaderId)
        {
            var filingHeader = UnitOfWork.Context.Set<TruckFilingHeader>()?.FirstOrDefault(x => x.Id == filingHeaderId);
            if (filingHeader != null)
            {
                return filingHeader.TruckDocuments.Union(filingHeader.TruckInbounds.SelectMany(y => y.Documents));
            }
            return new List<TruckDocument>();
        }
    }
}