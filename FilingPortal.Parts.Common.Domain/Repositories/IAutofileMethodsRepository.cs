using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Common.InboundTypes;
using Framework.Domain;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Common.Domain.Repositories
{
    /// <summary>
    /// Provides methods for auto-filing
    /// </summary>
    public interface IAutofileMethodsRepository<TInboundType>: IRepository<TInboundType>
        where TInboundType : Entity, IAutoFilingEntity
    {
        /// <summary>
        /// Gets all records with AutoRefile set to true
        /// </summary>
        IEnumerable<TInboundType> GetAutoRefileRecords();
    }
}
