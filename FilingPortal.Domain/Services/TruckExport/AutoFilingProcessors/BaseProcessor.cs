using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories.TruckExport;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.Domain.Services.TruckExport.AutoFilingProcessors
{
    internal abstract class BaseProcessor : IUpdateRecordsProcessor
    {
        public event ErrorHandler Notify;

        private IUpdateRecordsProcessor _successor;

        private readonly ITruckExportRepository _inboundRepository;

        protected BaseProcessor(ITruckExportRepository inboundRepository)
        {
            _inboundRepository = inboundRepository;
        }

        public void SetSuccessor(IUpdateRecordsProcessor processor)
        {
            _successor = processor;
        }

        public async Task Process(IList<TruckExportRecord> records, AppUsersModel user)
        {
            if (records.Any())
            {
                var recordsToProcess = FindRecordsToProcess(records).ToList();
                var recordsToPass = records.Except(recordsToProcess);
                IList<TruckExportRecord> notProcessed = new List<TruckExportRecord>();
                if (recordsToProcess.Any())
                {
                    notProcessed = Run(recordsToProcess, user);
                    foreach (TruckExportRecord inboundRecord in recordsToProcess.Except(notProcessed))
                    {
                        inboundRecord.IsAutoProcessed = true;
                        _inboundRepository.Update(inboundRecord);
                    }

                    await _inboundRepository.SaveAsync();
                }

                if (_successor != null) await _successor.Process(recordsToPass.Union(notProcessed).ToList(), user);
            }
        }

        protected abstract IList<TruckExportRecord> FindRecordsToProcess(IList<TruckExportRecord> records);

        protected abstract IList<TruckExportRecord> Run(IList<TruckExportRecord> records, AppUsersModel user);

        protected void SendError(TruckExportRecord record, string message)
        {
            Notify?.Invoke(record, message);
        }
    }
}