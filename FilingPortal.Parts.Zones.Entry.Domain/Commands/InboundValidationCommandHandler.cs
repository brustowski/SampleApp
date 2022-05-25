using FilingPortal.Parts.Zones.Entry.Domain.Repositories;
using Framework.Domain.Commands;

namespace FilingPortal.Parts.Zones.Entry.Domain.Commands
{
    /// <summary>
    /// Handles validate command for inbound records
    /// </summary>
    internal class InboundValidationCommandHandler : ICommandHandler<InboundValidationCommand>
    {
        /// <summary>
        /// The repository of inbound entities
        /// </summary>
        private readonly IInboundReadModelRepository _inboundReadModelRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundValidationCommandHandler"/> class
        /// </summary>
        /// <param name="inboundReadModelRepository">The repository of inbound entities</param>
        public InboundValidationCommandHandler(IInboundReadModelRepository inboundReadModelRepository)
            => _inboundReadModelRepository = inboundReadModelRepository;

        /// <summary>
        /// Handles command
        /// </summary>
        /// <param name="command">Mass delete command</param>
        public CommandResult Handle(InboundValidationCommand command)
        {
            _inboundReadModelRepository.Validate(command.RecordIds);

            return CommandResult.Ok;
        }
    }
}
