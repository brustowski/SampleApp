using FilingPortal.Domain.Repositories.Pipeline;
using Framework.Domain.Commands;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Domain.Commands.Handlers
{
    /// <summary>
    /// Handles mass delete command for pipeline records
    /// </summary>
    internal class PipelineInboundMassDeleteCommandHandler : ICommandHandler<PipelineInboundMassDeleteCommand>
    {
        /// <summary>
        /// The repository of Pipeline inbound entities
        /// </summary>
        private readonly IPipelineInboundReadModelRepository _pipelineInboundReadModelRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInboundMassDeleteCommandHandler{TInboundModel}"/> class
        /// </summary>
        /// <param name="pipelineInboundReadModelRepository">The repository of Pipeline inbound entities</param>
        public PipelineInboundMassDeleteCommandHandler(IPipelineInboundReadModelRepository pipelineInboundReadModelRepository) => _pipelineInboundReadModelRepository = pipelineInboundReadModelRepository;

        /// <summary>
        /// Handles command
        /// </summary>
        /// <param name="command">Mass delete command</param>
        public CommandResult Handle(PipelineInboundMassDeleteCommand command)
        {
            var errorResults = new List<int>();
            foreach (var recordId in command.RecordIds)
            {
                try
                {
                    _pipelineInboundReadModelRepository.DeleteById(recordId);

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
