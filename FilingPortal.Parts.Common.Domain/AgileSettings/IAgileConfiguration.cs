using System.Collections.Generic;

namespace FilingPortal.Parts.Common.Domain.AgileSettings
{
    /// <summary>
    /// Describes agile grid configuration
    /// </summary>
    /// <typeparam name="T">Type of the grid data</typeparam>
    public interface IAgileConfiguration<T>
    {
        /// <summary>
        /// Get the columns configuration <see cref="AgileField"/> for the grid
        /// </summary>
        IEnumerable<AgileField> GetFields();
    }
}
