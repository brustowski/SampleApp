using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Repositories.VesselImport;
using Framework.Domain.Commands;
using System;

namespace FilingPortal.Domain.Commands.Handlers
{
    /// <summary>
    /// Adds or updates record to Vessel imports
    /// </summary>
    public class VesselImportAddOrDeleteCommandHandler : ICommandHandler<VesselImportAddOrUpdateCommand>
    {
        private const string ErrCantUpdateRecord = "Vessel import record can't be updated";
        private const string ErrRecordNotFound = "Vessel import record with id = {0} not found";
        private const string ErrWrongRecord = "Vessel import record can't be null";

        /// <summary>
        /// The repository of Vessel import entities (Vessel Inbound records)
        /// </summary>
        private readonly IVesselImportReadModelRepository _vesselImportReadModelRepository;

        /// <summary>
        /// Vessel initial records repository
        /// </summary>
        private readonly IVesselImportRepository _vesselImportRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportMassDeleteCommandHandler"/> class
        /// </summary>
        /// <param name="vesselImportReadModelRepository">The repository of Vessel import entities (Vessel Inbound records)</param>
        /// <param name="vesselImportRepository">The repository for initial vessel records</param>
        public VesselImportAddOrDeleteCommandHandler(IVesselImportReadModelRepository vesselImportReadModelRepository, IVesselImportRepository vesselImportRepository)
        {
            _vesselImportReadModelRepository = vesselImportReadModelRepository;
            _vesselImportRepository = vesselImportRepository;
        }

        /// <summary>
        /// Executes command
        /// </summary>
        /// <param name="command">Underlying command</param>
        public CommandResult Handle(VesselImportAddOrUpdateCommand command)
        {
            if (command.Record == null)
            {
                return CommandResult.Failed(ErrWrongRecord);
            }

            if (command.RecordId.HasValue)
            {
                VesselImportReadModel readModelRecord = _vesselImportReadModelRepository.Get(command.RecordId.Value);
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
                _vesselImportRepository.AddOrUpdate(command.Record);
                _vesselImportRepository.Save();
            }
            catch (Exception e)
            {
                return CommandResult.Failed(e.Message);
            }
            return new CommandResult<int>(command.Record.Id);
        }
    }
}
