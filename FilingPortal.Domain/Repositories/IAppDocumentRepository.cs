using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories
{
    /// <summary>
    /// Interface for repository of application document
    /// </summary>
    public interface IAppDocumentRepository : IRepository<AppDocument>
    {
        /// <summary>
        /// Delete all documents for the specified user and file type
        /// </summary>
        /// <param name="user">The user login</param>
        /// <param name="fileType">The file type</param>
        void Delete(string user, string fileType);

        /// <summary>
        /// Deletes document with specified identifier
        /// </summary>
        /// <param name="id">The document identifier</param>
        void Delete(int id);

        /// <summary>
        /// Gets file for specified user and file type
        /// </summary>
        /// <param name="user">The user login</param>
        /// <param name="fileType">The file type</param>
        AppDocument GetUserDocument(string user, string fileType);

        /// <summary>
        /// Gets the file specified by identifier
        /// </summary>
        /// <param name="id">The file id</param>
        AppDocument GetDocument(int id);

        /// <summary>
        /// Check if the file exists for specified user and file type
        /// </summary>
        /// <param name="user">The user login</param>
        /// <param name="fileType">The file type</param>
        bool IsExist(string user, string fileType);

        /// <summary>
        /// Check if the file exists for specified identifier
        /// </summary>
        /// <param name="id">The document identifier</param>
        bool IsExist(int id);
    }
}