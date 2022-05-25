using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Rail.Export.Domain.Repositories;
using Framework.Domain.Commands;
using Framework.Infrastructure;

namespace FilingPortal.Parts.Rail.Export.Domain.Commands
{
    /// <summary>
    /// Handles mass delete command for inbond records
    /// </summary>
    internal class InboundMassDeleteCommandHandler : ICommandHandler<InboundMassDeleteCommand>
    {
        /// <summary>
        /// The repository of inbound entities
        /// </summary>
        private readonly IInboundReadModelRepository _inboundReadModelRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundMassDeleteCommandHandler"/> class
        /// </summary>
        /// <param name="inboundReadModelRepository">The repository of inbound entities</param>
        public InboundMassDeleteCommandHandler(IInboundReadModelRepository inboundReadModelRepository) 
            => _inboundReadModelRepository = inboundReadModelRepository;

        /// <summary>
        /// Handles command
        /// </summary>
        /// <param name="command">Mass delete command</param>
        public CommandResult Handle(InboundMassDeleteCommand command)
        {
            var errorResults = new List<int>();
            foreach (var recordId in command.RecordIds)
            {
                try
                {
                    _inboundReadModelRepository.DeleteById(recordId);

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
