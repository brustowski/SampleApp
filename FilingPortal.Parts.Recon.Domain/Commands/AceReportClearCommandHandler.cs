using FilingPortal.Parts.Recon.Domain.Repositories;
using Framework.Domain.Commands;
using Framework.Infrastructure;
using System;

namespace FilingPortal.Parts.Recon.Domain.Commands
{
    /// <summary>
    /// Handles ACE Report clear command
    /// </summary>
    internal class AceReportClearCommandHandler : ICommandHandler<AceReportClearCommand>
    {
        /// <summary>
        /// The ACE Report repository
        /// </summary>
        private readonly IAceReportRecordsRepository _aceReportRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AceReportClearCommandHandler"/> class
        /// </summary>
        /// <param name="aceReportRepository">The ACE Report repository</param>
        public AceReportClearCommandHandler(IAceReportRecordsRepository aceReportRepository)
        {
            _aceReportRepository = aceReportRepository;
        }

        /// <summary>
        /// Handles command
        /// </summary>
        /// <param name="command">The <see cref="AceReportClearCommand"/> command</param>
        public CommandResult Handle(AceReportClearCommand command)
        {
            try
            {
                _aceReportRepository.Clear();

                AppLogger.Info($"ACE Report was cleared successfully");
                return CommandResult.Ok;
            }
            catch (Exception e)
            {
                AppLogger.Error(e.Message);
                return CommandResult.Failed($"Unable to clear ACE Report");
            }


        }
    }
}
