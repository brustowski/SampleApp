using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Common.InboundTypes;
using Framework.Domain;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Common.Domain.Repositories
{
    /// <summary>
    /// Provides methods for validating inbound records data
    /// </summary>
    public interface IValidationRepository<TInboundType> : IRepository<TInboundType>
        where TInboundType : Entity, IValidationRequiredEntity
    {
        /// <summary>
        /// Runs Database validation on records
        /// </summary>
        /// <param name="records">Records</param>
        void Validate(IList<TInboundType> records);

        /// <summary>
        /// Runs Database validation of records
        /// </summary>
        /// <param name="recordIds">Records ids</param>
        void Validate(IEnumerable<int> recordIds);
    }
}
