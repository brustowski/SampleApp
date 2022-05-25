using System.Collections.Generic;

namespace FilingPortal.Domain.Repositories
{
    /// <summary>
    /// Defines the HTS Numbers Repository
    /// </summary>

    public interface IHtsNumbersRepository
    {
        /// <summary>
        /// Gets list of HTS numbers
        /// </summary>
        /// <param name="search">The search data</param>
        /// <param name="count">The search limit</param>
        /// <param name="htsDigits">Length of HTS number</param>
        IList<string> GetHtsNumbers(string search, int count, int htsDigits = 6);
    }
}