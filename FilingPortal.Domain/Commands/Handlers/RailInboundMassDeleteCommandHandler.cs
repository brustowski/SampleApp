using FilingPortal.Domain.Repositories.Rail;
using Framework.Domain.Commands;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Domain.Commands.Handlers
{
    internal class RailInboundMassDeleteCommandHandler : ICommandHandler<RailInboundMassDeleteCommand>
    {
        /// <summary>
        /// The repository of Rail Bd Parsed entities (Rail Inbound records)
        /// </summary>
        private readonly IRailInboundReadModelRepository _railInboundReadModelRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundMassDeleteCommandHandler{TInboundModel}"/> class
        /// </summary>
        /// <param name="railInboundReadModelRepository">The repository of Rail Bd Parsed entities (Rail Inbound records)</param>
        public RailInboundMassDeleteCommandHandler(IRailInboundReadModelRepository railInboundReadModelRepository) => _railInboundReadModelRepository = railInboundReadModelRepository;

        public CommandResult Handle(RailInboundMassDeleteCommand command)
        {
            var errorResults = new List<int>();
            foreach (var recordId in command.RecordIds)
            {
                try
                {
                    _railInboundReadModelRepository.DeleteById(recordId);

                    AppLogger.Info($"Inbound record with id = {recordId} was marked as deleted successfully");
                }
                catch (Exception e)
                {
                    AppLogger.Error(e.Message);
                    errorResults.Add(recordId);
                }
            }

            if (errorResults.Any())
            {
                return CommandResult.Failed($"Unable to delete records with ids: {string.Join(", ", errorResults.ToArray())}");
            }
            return CommandResult.Ok;
        }
    }
}
