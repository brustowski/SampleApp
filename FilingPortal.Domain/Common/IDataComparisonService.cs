using System.Collections.Generic;

namespace FilingPortal.Domain.Common
{
    /// <summary>
    /// Describes service to compare collections with different types
    /// </summary>
    /// <typeparam name="TFirst">First collection elements type</typeparam>
    /// <typeparam name="TSecond">Second collection elements type</typeparam>
    public interface IDataComparisonService<TFirst, in TSecond>
    {
        /// <summary>
        /// Сounts the number of matching items
        /// </summary>
        /// <param name="first">The first collection</param>
        /// <param name="second">The second collection</param>
        int Count(IEnumerable<TFirst> first, IEnumerable<TSecond> second);

        /// <summary>
        /// Gets matching items
        /// </summary>
        /// <param name="first">Result collection</param>
        /// <param name="second">Template collection</param>
        /// <returns></returns>
        IEnumerable<TFirst> GetMatches(IEnumerable<TFirst> first, IEnumerable<TSecond> second);
    }
}
