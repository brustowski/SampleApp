﻿using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using Framework.DataLayer;

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Repositories
{
    /// <summary>
    /// Class for repository of <see cref="Document"/>
    /// </summary>
    public class DocumentsRepository : BaseDocumentRepository<Document>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentsRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public DocumentsRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Gets the list of documents by filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public override IEnumerable<Document> GetListByFilingHeader(int filingHeaderId)
        {
            FilingHeader filingHeader = UnitOfWork.Context.Set<FilingHeader>()?.FirstOrDefault(x => x.Id == filingHeaderId);
            if (filingHeader != null)
            {
                return filingHeader.Documents;
            }
            return new List<Document>();
        }
    }
}