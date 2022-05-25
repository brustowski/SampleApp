using System;
using FilingPortal.Domain.Repositories.Rail;
using Framework.Domain.Commands;

namespace FilingPortal.Domain.Commands.Handlers
{
    /// <summary>
    /// Adds or updates Rail inbound record
    /// </summary>
    public class RailInboundAddOrDeleteCommandHandler : ICommandHandler<RailImportAddOrUpdateCommand>
    {
        private const string ErrCantUpdateRecord = "Rail manifest can't be updated";
        private const string ErrRecordNotFound = "Rail manifest with id = {0} not found";
        private const string ErrWrongRecord = "Rail manifest can't be null";

        /// <summary>
        /// The repository of Rail entities
        /// </summary>
        private readonly IRailInboundReadModelRepository _railReadModelRepository;

        /// <summary>
        /// Rail initial records repository
        /// </summary>
        private readonly IBdParsedRepository _railRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="RailInboundAddOrDeleteCommandHandler"/> class
        /// </summary>
        /// <param name="railReadModelRepository">The repository of Rail entities</param>
        /// <param name="railRepository">Rail initial records repository</param>
        public RailInboundAddOrDeleteCommandHandler(IRailInboundReadModelRepository railReadModelRepository, IBdParsedRepository railRepository)
        {
            _railReadModelRepository = railReadModelRepository;
            _railRepository = railRepository;
        }

        /// <summary>
        /// Executes command
        /// </summary>
        /// <param name="command">Underlying command</param>
        public CommandResult Handle(RailImportAddOrUpdateCommand command)
        {
            if (command.Record == null)
            {
                return CommandResult.Failed(ErrWrongRecord);
            }

            if (command.RecordId.HasValue)
            {
                var readModelRecord = _railReadModelRepository.Get(command.RecordId.Value);
                if (readModelRecord == null)
                {
                    return CommandResult.Failed(string.Format(ErrRecordNotFound, command.RecordId));
                }

                if (readModelRecord.CanEditInitialRecord())
                {
                    command.Record.Id = command.RecordId.Value;
                }
                else
                {
                    return CommandResult.Failed(ErrCantUpdateRecord);
                }
            }
            try
            {
                _railRepository.AddOrUpdate(command.Record);
                _railRepository.Save();
            }
            catch (Exception e)
            {
                return CommandResult.Failed(e.Message);
            }
            return new CommandResult<int>(command.Record.Id);
        }
    }
}
