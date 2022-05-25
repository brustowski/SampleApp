using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories.Truck;
using Framework.Domain.Commands;
using Framework.Domain.Repositories;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Domain.Commands.Handlers
{
    internal class TruckInboundMassDeleteCommandHandler : ICommandHandler<TruckInboundMassDeleteCommand>
    {
        /// <summary>
        /// The repository of Truck Bd Parsed entities (Truck Inbound records)
        /// </summary>
        private readonly ITruckInboundReadModelRepository _truckInboundReadModelRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundMassDeleteCommandHandler{TInboundModel}"/> class
        /// </summary>
        /// <param name="truckInboundReadModelRepository">The repository of Truck Bd Parsed entities (Truck Inbound records)</param>
        public TruckInboundMassDeleteCommandHandler(ITruckInboundReadModelRepository truckInboundReadModelRepository) => _truckInboundReadModelRepository = truckInboundReadModelRepository;

        public CommandResult Handle(TruckInboundMassDeleteCommand command)
        {
            var errorResults = new List<int>();
            foreach (var recordId in command.RecordIds)
            {
                try
                {
                    _truckInboundReadModelRepository.DeleteById(recordId);

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
