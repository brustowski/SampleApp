namespace FilingPortal.Domain.Commands.Handlers
{
    using FilingPortal.Domain.Commands;
    using FilingPortal.Domain.Repositories.VesselExport;
    using Framework.Domain.Commands;
    using Framework.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides Command handler for the <see cref="VesselExportMassDeleteCommand" /> command
    /// </summary>
    internal class VesselExportMassDeleteCommandHandler : ICommandHandler<VesselExportMassDeleteCommand>
    {
        /// <summary>
        /// The repository of Vessel Export entities
        /// </summary>
        private readonly IVesselExportReadModelRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportMassDeleteCommandHandler"/> class.
        /// </summary>
        /// <param name="repository">The repository of the Vessel Export entities</param>
        public VesselExportMassDeleteCommandHandler(IVesselExportReadModelRepository repository) => _repository = repository;

        /// <summary>
        /// Handle the command
        /// </summary>
        /// <param name="command">The command <see cref="VesselExportMassDeleteCommand"/></param>
        public CommandResult Handle(VesselExportMassDeleteCommand command)
        {
            var errorResults = new List<int>();
            foreach (var recordId in command.RecordIds)
            {
                try
                {
                    _repository.DeleteById(recordId);

                    AppLogger.Info($"Record with id = {recordId} was marked as deleted successfully");
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
