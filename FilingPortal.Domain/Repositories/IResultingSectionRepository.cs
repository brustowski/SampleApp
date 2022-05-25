using System;

namespace FilingPortal.Domain.Repositories
{
    /// <summary>
    /// Defines Resulting Section repository
    /// </summary>
    [Obsolete]
    public interface IResultingSectionRepository
    {
        /// <summary>
        /// Add new row to the table corresponding to specified section
        /// </summary>
        /// <param name="sectionName">Section name</param>
        /// <param name="filingHeaderId">Filing Header id</param>
        /// <param name="parentId">Parent section id</param>
        /// <param name="userAccount">User account, under which credentials process is executed</param>
        /// <param name="uniqueId">Unique operation id</param>
        int AddSectionRow(string sectionName, int filingHeaderId, int parentId, string userAccount = null, Guid? uniqueId = null);

        /// <summary>
        /// Delete record with specified id from the table corresponding to the specified section name
        /// </summary>
        /// <param name="sectionName">Section name</param>
        /// <param name="recordId">Record id</param>
        void DeleteSectionRecord(string sectionName, int recordId);
    }
}