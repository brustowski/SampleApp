using System;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Isf.Domain.Repositories;
using Framework.Domain.Commands;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Isf.Domain.Commands
{
    /// <summary>
    /// Adds or edits record in repository
    /// </summary>
    public class InboundAddOrEditCommandHandler : ICommandHandler<InboundAddOrEditCommand>
    {
        private const string ErrCantUpdateRecord = "ISF record can't be updated";
        private const string ErrRecordNotFound = "ISF record with id = {0} not found";
        private const string ErrWrongRecord = "Inbound record can't be null";

        /// <summary>
        /// Inbound records read model repository
        /// </summary>
        private readonly IInboundReadModelRepository _inboundReadModelRepository;

        /// <summary>
        /// Inbound records repository
        /// </summary>
        private readonly IInboundRecordsRepository _inboundRepository;

        /// <summary>
        /// Inbound manufacturers repository
        /// </summary>
        private readonly IManufacturersRepository _manufacturersRepository;
        /// <summary>
        /// Addresses repository
        /// </summary>
        private readonly IRepository<AppAddress> _addressesRepository;
        /// <summary>
        /// Bills repository
        /// </summary>
        private readonly IBillsRepository _billsRepository;
        /// <summary>
        /// Containers repository
        /// </summary>
        private readonly IContainersRepository _containersRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="InboundAddOrEditCommandHandler"/> class
        /// </summary>
        /// <param name="inboundReadModelRepository">The repository for inbound read models</param>
        /// <param name="inboundRepository">The repository for inbound records</param>
        /// <param name="manufacturersRepository">The repository for inbound manufacturers</param>
        /// <param name="addressesRepository">Addresses repository</param>
        /// <param name="billsRepository">Bills repository</param>
        /// <param name="containersRepository">Containers Repository</param>
        public InboundAddOrEditCommandHandler(IInboundReadModelRepository inboundReadModelRepository,
            IInboundRecordsRepository inboundRepository,
            IManufacturersRepository manufacturersRepository,
            IRepository<AppAddress> addressesRepository,
            IBillsRepository billsRepository,
            IContainersRepository containersRepository)
        {
            _inboundReadModelRepository = inboundReadModelRepository;
            _inboundRepository = inboundRepository;
            _manufacturersRepository = manufacturersRepository;
            _addressesRepository = addressesRepository;
            _billsRepository = billsRepository;
            _containersRepository = containersRepository;
        }

        /// <summary>
        /// Executes command
        /// </summary>
        /// <param name="command">Underlying command</param>
        public CommandResult Handle(InboundAddOrEditCommand command)
        {
            if (command.Record == null)
            {
                return CommandResult.Failed(ErrWrongRecord);
            }
            if (command.Record.Id != 0)
            {
                Entities.InboundReadModel readModelRecord = _inboundReadModelRepository.Get(command.Record.Id);
                if (readModelRecord == null)
                {
                    return CommandResult.Failed(string.Format(ErrRecordNotFound, command.Record.Id));
                }

                if (!readModelRecord.CanEditInitialRecord())
                {
                    return CommandResult.Failed(ErrCantUpdateRecord);
                }
            }
            try
            {
                _manufacturersRepository.RemoveManufacturers(command.Record);
                foreach (Entities.InboundManufacturerRecord manufacturer in command.Record.Manufacturers)
                {
                    manufacturer.InboundRecordId = command.Record.Id;
                    manufacturer.CreatedUser = command.Record.CreatedUser;
                    _manufacturersRepository.Add(manufacturer);
                }

                _billsRepository.RemoveBills(command.Record);
                foreach (Entities.InboundBillRecord bill in command.Record.Bills)
                {
                    bill.InboundRecordId = command.Record.Id;
                    bill.CreatedUser = command.Record.CreatedUser;
                    _billsRepository.Add(bill);
                }

                _containersRepository.RemoveContainers(command.Record);
                foreach (Entities.InboundContainerRecord container in command.Record.Containers)
                {
                    container.InboundRecordId = command.Record.Id;
                    container.CreatedUser = command.Record.CreatedUser;
                    _containersRepository.Add(container);
                }

                UpdateAddress(command.Record.BuyerAppAddress);
                UpdateAddress(command.Record.ConsolidatorAppAddress);
                UpdateAddress(command.Record.ContainerStuffingLocationAppAddress);
                UpdateAddress(command.Record.SellerAppAddress);
                UpdateAddress(command.Record.ShipToAppAddress);

                _addressesRepository.Save();

                command.Record.BuyerAppAddressId = command.Record.BuyerAppAddress?.Id;
                command.Record.ConsolidatorAppAddressId = command.Record.ConsolidatorAppAddress?.Id;
                command.Record.ContainerStuffingLocationAppAddressId = command.Record.ContainerStuffingLocationAppAddress?.Id;
                command.Record.SellerAppAddressId = command.Record.SellerAppAddress?.Id;
                command.Record.ShipToAppAddressId = command.Record.ShipToAppAddress?.Id;

                _inboundRepository.AddOrUpdate(command.Record);

                _inboundRepository.Save();
            }
            catch (Exception e)
            {
                return CommandResult.Failed(e.Message);
            }
            return new CommandResult<int>(command.Record.Id);
        }

        private void UpdateAddress(AppAddress appAddress)
        {
            if (appAddress != null)
                _addressesRepository.AddOrUpdate(appAddress);
        }
    }
}
