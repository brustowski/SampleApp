using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories.TruckExport;
using FilingPortal.Parts.Common.Domain.Enums;
using Framework.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Domain.Services.TruckExport
{
    /// <summary>
    /// Provides methods to work with Job Numbers
    /// </summary>
    internal class TruckExportJobNumberService : ITruckExportJobNumberService
    {
        /// <summary>
        /// Inbound records repository
        /// </summary>
        private readonly ITruckExportRepository _inboundRepository;

        /// <summary>
        /// The filing headers repository
        /// </summary>
        private readonly ITruckExportFilingHeadersRepository _filingHeaderRepository;

        /// <summary>
        /// The filing procedure service
        /// </summary>
        private readonly ITruckExportFilingService _procedureService;

        /// <summary>
        /// Initialize the new instance of the <see cref="TruckExportJobNumberService"/> class
        /// </summary>
        /// <param name="inboundRepository">The inbound records repository</param>
        /// <param name="filingHeaderRepository">The filing headers repository</param>
        /// <param name="procedureService">The filing procedure service</param>
        public TruckExportJobNumberService(ITruckExportRepository inboundRepository,
            ITruckExportFilingHeadersRepository filingHeaderRepository,
            ITruckExportFilingService procedureService)
        {
            _inboundRepository = inboundRepository;
            _filingHeaderRepository = filingHeaderRepository;
            _procedureService = procedureService;
        }

        /// <summary>
        /// Ensure Job Number for specified records
        /// </summary>
        /// <param name="recordIds">Array of the records to validate</param>
        /// <param name="userId">The user id</param>
        public int EnsureJobNumberForRecords(int[] recordIds, string userId)
        {
            using (new MonitoredScope("Starting job number ensuring process"))
            {
                // Let's find existing filing headers
                var headers = new List<TruckExportFilingHeader>();
                var processedRecordsCount = 0;

                var records = _inboundRepository.GetList(recordIds).ToList();
                _inboundRepository.Validate(records);

                foreach (TruckExportRecord record in records)
                {
                    var jobNumber = _inboundRepository.TryGetJobNumber(record);
                    if (string.IsNullOrWhiteSpace(jobNumber)) continue;
                    TruckExportFilingHeader header =
                        record.FilingHeaders.FirstOrDefault(x => x.JobStatus != JobStatus.Open);
                    if (header == null)
                    {
                        if (!record.ValidationPassed)
                        {
                            continue;
                        }

                        OperationResultWithValue<int[]> newFilingHeaders = _procedureService.CreateSingleFilingFilingHeaders(new[] {record.Id}, userId);
                        if (!newFilingHeaders.IsValid) continue;

                        header = _filingHeaderRepository.Get(newFilingHeaders.Value.FirstOrDefault());
                        header.SetUpdatedStatus();
                    }

                    header.FilingNumber = jobNumber;
                    headers.Add(header);
                    processedRecordsCount++;
                }

                foreach (TruckExportFilingHeader header in headers)
                {
                    _filingHeaderRepository.Update(header);
                }

                _filingHeaderRepository.Save();

                return processedRecordsCount;
            }
        }
    }
}