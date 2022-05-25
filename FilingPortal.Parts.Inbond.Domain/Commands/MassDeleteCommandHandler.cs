using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Inbond.Domain.Repositories;
using Framework.Domain.Commands;
using Framework.Infrastructure;

namespace FilingPortal.Parts.Inbond.Domain.Commands
{
    /// <summary>
    /// Handles mass delete command for inbond records
    /// </summary>
    internal class MassDeleteCommandHandler : ICommandHandler<InbondMassDeleteCommand>
    {
        /// <summary>
        /// The repository of inbound entities
        /// </summary>
        private readonly IInboundReadModelRepository _inbondReadModelRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MassDeleteCommandHandler"/> class
        /// </summary>
        /// <param name="inbondReadModelRepository">The repository of inbound entities</param>
        public MassDeleteCommandHandler(IInboundReadModelRepository inbondReadModelRepository) => _inbondReadModelRepository = inbondReadModelRepository;

        /// <summary>
        /// Handles command
        /// </summary>
        /// <param name="command">Mass delete command</param>
        public CommandResult Handle(InbondMassDeleteCommand command)
        {
            var errorResults = new List<int>();
            foreach (var recordId in command.RecordIds)
            {
                try
                {
                    _inbondReadModelRepository.DeleteById(recordId);

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
