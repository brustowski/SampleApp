using System.Collections.Generic;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Describes the service to resolve collection of the compatible data types
    /// </summary>
    public interface ICompatibleDataTypeService
    {
        /// <summary>
        /// Gets collection of the compatible data types for the specified type
        /// </summary>
        /// <param name="type">The type</param>
        IEnumerable<string> Get(string type);

        /// <summary>
        /// Gets collection of the compatible data types
        /// </summary>
        IEnumerable<string> GetAll();
    }
}
