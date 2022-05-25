using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories.TruckExport;
using Framework.Domain.Commands;
using Framework.Infrastructure;

namespace FilingPortal.Domain.Commands.Handlers
{

    /// <summary>
    /// Provides Command handler for the <see cref="TruckExportMassDeleteCommand" /> command
    /// </summary>
    internal class TruckExportMassDeleteCommandHandler : ICommandHandler<TruckExportMassDeleteCommand>
    {
        /// <summary>
        /// The repository of Truck Export entities
        /// </summary>
        private readonly ITruckExportReadModelRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportMassDeleteCommandHandler"/> class.
        /// </summary>
        /// <param name="repository">The repository of the Truck Export entities</param>
        public TruckExportMassDeleteCommandHandler(ITruckExportReadModelRepository repository) => _repository = repository;

        /// <summary>
        /// Handle the command
        /// </summary>
        /// <param name="command">The command <see cref="TruckExportMassDeleteCommand"/></param>
        public CommandResult Handle(TruckExportMassDeleteCommand command)
        {
            var errorResults = new List<int>();
            IEnumerable<TruckExportReadModel> records = _repository.GetList(command.RecordIds);
            foreach (TruckExportReadModel record in records)
            {
                try
                {
                    if (record.CanBeDeleted())
                    {
                        _repository.DeleteById(record.Id);

                        AppLogger.Info($"Record with id = {record.Id} was marked as deleted successfully");
                    }
                }
                catch (Exception e)
                {
                    AppLogger.Error(e.Message);
                    errorResults.Add(record.Id);
                }
            }

            return errorResults.Any()
                ? CommandResult.Failed($"Unable to delete records with ids: {string.Join(", ", errorResults.ToArray())}")
                : CommandResult.Ok;
        }
    }
}
