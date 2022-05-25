using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Common.InboundTypes;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Framework.Domain;
using Framework.Domain.Repositories;

namespace FilingPortal.PluginEngine.Services.Filing.Auto.AutoFilingProcessors
{
    /// <summary>
    /// Processes just created records
    /// </summary>
    /// <typeparam name="TAutofileEntityType"></typeparam>
    internal class NotMatchedProcessor<TAutofileEntityType> : BaseProcessor<TAutofileEntityType>
    where TAutofileEntityType : Entity, IAutoFilingEntity
    {
        /// <summary>
        /// Creates a new instance of <see cref="NotMatchedProcessor{TAutofileEntityType}"/>
        /// </summary>
        /// <param name="inboundRepository"></param>
        public NotMatchedProcessor(IRepository<TAutofileEntityType> inboundRepository) : base(inboundRepository)
        {
        }

        /// <summary>
        /// Accepts all just created records
        /// </summary>
        /// <param name="records">Records</param>
        protected override IList<TAutofileEntityType> FindRecordsToProcess(IList<TAutofileEntityType> records)
        {
            return records.Where(x => !x.IsUpdate).ToList();
        }
        /// <summary>
        /// Processes records
        /// </summary>
        /// <param name="records">Records to process</param>
        /// <param name="user">User that executes processing</param>
        protected override IList<TAutofileEntityType> Run(IList<TAutofileEntityType> records, AppUsersModel user)
        {
            return new List<TAutofileEntityType>();
        }
    }
}
