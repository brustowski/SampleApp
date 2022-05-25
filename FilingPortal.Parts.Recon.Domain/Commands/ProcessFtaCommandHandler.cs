using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Enums;
using Framework.Domain.Commands;
using Framework.Domain.Repositories;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FilingPortal.Parts.Recon.Domain.Commands
{
    /// <summary>
    /// Handles FTA Recon process command
    /// </summary>
    internal class ProcessFtaCommandHandler : ICommandHandler<ProcessFtaCommand>
    {
        /// <summary>
        /// The FTA Recon repository
        /// </summary>
        private readonly ISearchRepository<FtaRecon> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessFtaCommandHandler"/> class
        /// </summary>
        /// <param name="repository">The FTA Recon repository</param>
        public ProcessFtaCommandHandler(ISearchRepository<FtaRecon> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Handles command
        /// </summary>
        /// <param name="command">The <see cref="AceReportClearCommand"/> command</param>
        public CommandResult Handle(ProcessFtaCommand command)
        {
            try
            {
                AppLogger.Info($"Processing of FTA Recon records started. Number of records: {command.Ids.Length}");
                var stopWatch = Stopwatch.StartNew();
                IEnumerable<FtaRecon> records = _repository.GetList(command.Ids);
                foreach (FtaRecon recon in records)
                {
                    recon.Status = (int)FtaReconStatusValue.Updated;
                }
                _repository.Save();

                stopWatch.Stop();
                AppLogger.Info($"TimeMeasureInfo: Processing of records took {stopWatch.ElapsedMilliseconds} ms [{stopWatch.ElapsedMilliseconds / 1000} s]");
                AppLogger.Info($"FTA Recon records processed successfully");
                return CommandResult.Ok;
            }
            catch (Exception e)
            {
                AppLogger.Error(e.Message);
                return CommandResult.Failed($"Unable to process FTA Recon records");
            }


        }
    }
}
