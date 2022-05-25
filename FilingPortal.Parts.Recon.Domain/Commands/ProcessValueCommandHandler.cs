using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Enums;
using Framework.Domain.Commands;
using Framework.Domain.Repositories;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using FilingPortal.Parts.Recon.Domain.Repositories;

namespace FilingPortal.Parts.Recon.Domain.Commands
{
    /// <summary>
    /// Handles Value Recon process command
    /// </summary>
    internal class ProcessValueCommandHandler : ICommandHandler<ProcessValueCommand>
    {
        /// <summary>
        /// The Value Recon repository
        /// </summary>
        private readonly ISearchRepository<ValueRecon> _valueRepository;

        /// <summary>
        /// The FTA Recon repository
        /// </summary>
        private readonly IFtaReconRepository _ftaRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessValueCommandHandler"/> class
        /// </summary>
        /// <param name="valueValueRepository">The Value Recon repository</param>
        /// <param name="ftaRepository">The FTA recon repository</param>
        public ProcessValueCommandHandler(ISearchRepository<ValueRecon> valueValueRepository, IFtaReconRepository ftaRepository)
        {
            _valueRepository = valueValueRepository;
            _ftaRepository = ftaRepository;
        }

        /// <summary>
        /// Handles command
        /// </summary>
        /// <param name="command">The <see cref="AceReportClearCommand"/> command</param>
        public CommandResult Handle(ProcessValueCommand command)
        {
            try
            {
                AppLogger.Info($"Processing of Value Recon records started. Number of records: {command.Ids.Length}");
                var stopWatch = Stopwatch.StartNew();
                var count = 0;
                IEnumerable<ValueRecon> records = _valueRepository.GetList(command.Ids);
                foreach (ValueRecon recon in records)
                {
                    if (!_ftaRepository.PopulateFtaJobData(recon.Id)) continue;
                    count++;
                    recon.Status = (int)ValueReconStatusValue.Processed;
                }
                _valueRepository.Save();

                stopWatch.Stop();
                AppLogger.Info($"TimeMeasureInfo: Processing of records took {stopWatch.ElapsedMilliseconds} ms [{stopWatch.ElapsedMilliseconds / 1000} s]");
                AppLogger.Info($"Value Recon records processed successfully");
                return new CommandResult<int>(count);
            }
            catch (Exception e)
            {
                AppLogger.Error(e.Message);
                return CommandResult.Failed($"Unable to process Value Recon records");
            }


        }
    }
}
