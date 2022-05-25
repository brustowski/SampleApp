using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Repositories
{
    /// <summary>
    /// Describes repository of InboundXml objects
    /// </summary>
    public interface IInboundXmlRepository: ISearchRepository<InboundXml>
    {
        /// <summary>
        /// Returns files to process
        /// </summary>
        Task<IList<InboundXml>> GetUnprocessedFiles();
    }
}
