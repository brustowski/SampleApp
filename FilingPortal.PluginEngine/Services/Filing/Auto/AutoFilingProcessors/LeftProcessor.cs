using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Common.InboundTypes;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Framework.Domain;
using Framework.Domain.Repositories;

namespace FilingPortal.PluginEngine.Services.Filing.Auto.AutoFilingProcessors
{
    /// <summary>
    /// Processes all records that were not processed by other processors
    /// </summary>
    /// <typeparam name="TAutoFilingType">Auto-filing entity</typeparam>
    internal class LeftProcessor<TAutoFilingType> : BaseProcessor<TAutoFilingType>
        where TAutoFilingType : Entity, IAutoFilingEntity
    {
        /// <summary>
        /// Creates new instance of <see cref="LeftProcessor{TAutoFilingType}"/>
        /// </summary>
        /// <param name="inboundRepository">Repository of auto-file entity</param>
        public LeftProcessor(IRepository<TAutoFilingType> inboundRepository) : base(inboundRepository)
        {
        }

        /// <summary>
        /// Accepts all records
        /// </summary>
        /// <param name="records">Records</param>
        protected override IList<TAutoFilingType> FindRecordsToProcess(IList<TAutoFilingType> records)
        {
            return records;
        }

        /// <summary>
        /// Processes records
        /// </summary>
        /// <param name="records">Records to process</param>
        /// <param name="user">User that executes processing</param>
        protected override IList<TAutoFilingType> Run(IList<TAutoFilingType> records, AppUsersModel user)
        {
            foreach (TAutoFilingType truckExportRecord in records)
            {
                SendError(truckExportRecord, "Record was not processed for some reason");
            }

            return new List<TAutoFilingType>();
        }


    }
}