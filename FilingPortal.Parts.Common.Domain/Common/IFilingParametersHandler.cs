using FilingPortal.Parts.Common.Domain.DTOs;

namespace FilingPortal.Parts.Common.Domain.Common
{
    /// <summary>
    /// Describes the Filing parameters handlers
    /// </summary>
    public interface IFilingParametersHandler
    {
        /// <summary>
        /// Executes additional actions on provided parameter
        /// </summary>
        /// <param name="parameter">The parameter to process</param>
        /// <param name="additional">List of the additional data</param>
        void Process(InboundRecordParameter parameter, params object[] additional);
    }
}
