using System;

namespace FilingPortal.Parts.Common.Domain.Repositories
{
    /// <summary>
    /// Defines Filing Section repository
    /// </summary>
    public interface IFilingSectionRepository
    {
        /// <summary>
        /// Add new row to the table corresponding to specified section
        /// </summary>
        /// <param name="sectionName">Section name</param>
        /// <param name="filingHeaderId">Filing Header id</param>
        /// <param name="parentId">Parent section id</param>
        /// <param name="userAccount">User account, under which credentials process is executed</param>
        Guid AddSectionRecord(string sectionName, int filingHeaderId, int parentId, string userAccount = null);

        /// <summary>
        /// Delete record with specified id from the table corresponding to the specified section name
        /// </summary>
        /// <param name="sectionName">Section name</param>
        /// <param name="recordId">Record id</param>
        void DeleteSectionRecord(string sectionName, int recordId);
    }
}