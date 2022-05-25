using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Parts.Common.Domain.Common.InboundTypes;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Framework.Domain;
using Framework.Domain.Repositories;

namespace FilingPortal.PluginEngine.Services.Filing.Auto.AutoFilingProcessors
{
    /// <summary>
    /// Base class for all records processing
    /// </summary>
    /// <typeparam name="TInboundType">Auto-file entity</typeparam>
    internal abstract class BaseProcessor<TInboundType> : IUpdateRecordsProcessor<TInboundType>
        where TInboundType : Entity, IAutoFilingEntity
    {
        /// <summary>
        /// Error handler
        /// </summary>
        public event ErrorHandler<TInboundType> Notify;

        /// <summary>
        /// Successor
        /// </summary>
        private IUpdateRecordsProcessor<TInboundType> _successor;

        /// <summary>
        /// Auto-file records repository
        /// </summary>
        private readonly IRepository<TInboundType> _inboundRepository;

        /// <summary>
        /// Creates a new instance of <see cref="BaseProcessor{TInboundType}"/>
        /// </summary>
        /// <param name="inboundRepository"></param>
        protected BaseProcessor(IRepository<TInboundType> inboundRepository)
        {
            _inboundRepository = inboundRepository;
        }
        /// <summary>
        /// Sets up next processor for other records
        /// </summary>
        /// <param name="processor">Next processor</param>
        public void SetSuccessor(IUpdateRecordsProcessor<TInboundType> processor)
        {
            _successor = processor;
        }
        /// <summary>
        /// Processes records
        /// </summary>
        /// <param name="records">Records to process</param>
        /// <param name="user">User that executes processing</param>
        public async Task Process(IList<TInboundType> records, AppUsersModel user)
        {
            if (records.Any())
            {
                var recordsToProcess = FindRecordsToProcess(records).ToList();
                IEnumerable<TInboundType> recordsToPass = records.Except(recordsToProcess);
                IList<TInboundType> notProcessed = new List<TInboundType>();
                if (recordsToProcess.Any())
                {
                    notProcessed = Run(recordsToProcess, user);
                    foreach (TInboundType inboundRecord in recordsToProcess.Except(notProcessed))
                    {
                        inboundRecord.IsAutoProcessed = true;
                        _inboundRepository.Update(inboundRecord);
                    }

                    await _inboundRepository.SaveAsync();
                }

                if (_successor != null) await _successor.Process(recordsToPass.Union(notProcessed).ToList(), user);
            }
        }

        /// <summary>
        /// Finds corresponding records that processor is able to process
        /// </summary>
        /// <param name="records">All records</param>
        protected abstract IList<TInboundType> FindRecordsToProcess(IList<TInboundType> records);

        /// <summary>
        /// Processes records
        /// </summary>
        /// <param name="records">Records to process</param>
        /// <param name="user">User that executes processing</param>
        protected abstract IList<TInboundType> Run(IList<TInboundType> records, AppUsersModel user);

        /// <summary>
        /// Sends notifications in record processing
        /// </summary>
        /// <param name="record">Record</param>
        /// <param name="message">Notification message</param>
        protected void SendError(TInboundType record, string message)
        {
            Notify?.Invoke(record, message);
        }
    }
}