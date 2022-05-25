using FilingPortal.Domain.Repositories.VesselExport;
using Framework.Domain.Commands;
using System;
using FilingPortal.Domain.Entities.VesselExport;

namespace FilingPortal.Domain.Commands.Handlers
{
    /// <summary>
    /// Adds or updates record to Vessel exports
    /// </summary>
    public class VesselExportAddOrDeleteCommandHandler : ICommandHandler<VesselExportAddOrUpdateCommand>
    {
        private const string ErrCantUpdateRecord = "Vessel export record can't be updated";
        private const string ErrRecordNotFound = "Vessel export record with id = {0} not found";
        private const string ErrWrongRecord = "Vessel export record can't be null";

        /// <summary>
        /// The repository of Vessel export entities (Vessel Inbound records)
        /// </summary>
        private readonly IVesselExportReadModelRepository _vesselExportReadModelRepository;

        /// <summary>
        /// Vessel initial records repository
        /// </summary>
        private readonly IVesselExportRepository _vesselExportRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportMassDeleteCommandHandler"/> class
        /// </summary>
        /// <param name="vesselExportReadModelRepository">The repository of Vessel export entities (Vessel Inbound records)</param>
        /// <param name="vesselExportRepository">The repository for initial vessel records</param>
        public VesselExportAddOrDeleteCommandHandler(IVesselExportReadModelRepository vesselExportReadModelRepository, IVesselExportRepository vesselExportRepository)
        {
            _vesselExportReadModelRepository = vesselExportReadModelRepository;
            _vesselExportRepository = vesselExportRepository;
        }

        /// <summary>
        /// Executes command
        /// </summary>
        /// <param name="command">Underlying command</param>
        public CommandResult Handle(VesselExportAddOrUpdateCommand command)
        {
            if (command.Record == null)
            {
                return CommandResult.Failed(ErrWrongRecord);
            }

            if (command.RecordId.HasValue)
            {
                VesselExportReadModel readModelRecord = _vesselExportReadModelRepository.Get(command.RecordId.Value);
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
                _vesselExportRepository.AddOrUpdate(command.Record);
                _vesselExportRepository.Save();
            }
            catch (Exception e)
            {
                return CommandResult.Failed(e.Message);
            }
            return new CommandResult<int>(command.Record.Id);
        }
    }
}
