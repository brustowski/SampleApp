namespace FilingPortal.Domain.Enums
{
    using Reinforced.Typings.Attributes;
    using System.ComponentModel;

    /// <summary>
    /// Defines the Rail Inbound reocrd Status
    /// </summary>
    [TsEnum(IncludeNamespace = false)]
    public enum RailInboundRecordStatus : byte
    {
        /// <summary>
        /// Defines the Open status
        /// </summary>
        [Description("Open")]
        Open,

        /// <summary>
        /// Defines the Duplicates status
        /// </summary>
        [Description("Duplicated")]
        Duplicates,

        /// <summary>
        /// Defines the Archived status
        /// </summary>
        [Description("Archived")]
        Archived,

        /// <summary>
        /// Defines the Deleted status
        /// </summary>
        [Description("Deleted")]
        Deleted
    }
}
