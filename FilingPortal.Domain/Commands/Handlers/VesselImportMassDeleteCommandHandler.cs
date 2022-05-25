using FilingPortal.Domain.Repositories.VesselImport;
using Framework.Domain.Commands;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Domain.Commands.Handlers
{
    public class VesselImportMassDeleteCommandHandler : ICommandHandler<VesselImportMassDeleteCommand>
    {
        /// <summary>
        /// The repository of Vessel import entities (Vessel Inbound records)
        /// </summary>
        private readonly IVesselImportReadModelRepository _vesselImportReadModelRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportMassDeleteCommandHandler"/> class
        /// </summary>
        /// <param name="vesselImportReadModelRepository">The repository of Vessel import entities (Vessel Inbound records)</param>
        public VesselImportMassDeleteCommandHandler(IVesselImportReadModelRepository vesselImportReadModelRepository) => _vesselImportReadModelRepository = vesselImportReadModelRepository;

        public CommandResult Handle(VesselImportMassDeleteCommand command)
        {
            var errorResults = new List<int>();
            foreach (var recordId in command.RecordIds)
            {
                try
                {
                    _vesselImportReadModelRepository.DeleteById(recordId);

                    AppLogger.Info($"Import record with id = {recordId} was marked as deleted successfully");
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
