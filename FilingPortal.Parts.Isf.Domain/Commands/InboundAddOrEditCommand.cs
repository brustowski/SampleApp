using FilingPortal.Parts.Isf.Domain.Entities;
using Framework.Domain.Commands;

namespace FilingPortal.Parts.Isf.Domain.Commands
{
    /// <summary>
    /// Implements inbound record add or edit command
    /// </summary>
    public class InboundAddOrEditCommand : ICommand
    {
        /// <summary>
        /// Gets or sets import record
        /// </summary>
        public virtual InboundRecord Record { get; set; }
    }
}
