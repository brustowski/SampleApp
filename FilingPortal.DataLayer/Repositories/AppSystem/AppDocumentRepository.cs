using FilingPortal.Domain.Repositories;
using Framework.DataLayer;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.DataLayer.Repositories.AppSystem
{
    /// <summary>
    /// Defines the Application Document repository
    /// </summary>
    public class AppDocumentRepository : Repository<AppDocument>, IAppDocumentRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDocumentRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unitOfWork<see cref="IUnitOfWorkFactory"/></param>
        public AppDocumentRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Delete all documents for the specified user and file type
        /// </summary>
        /// <param name="user">The user login</param>
        /// <param name="fileType">The file type</param>
        public void Delete(string user, string fileType)
        {
            Set.RemoveRange(Set.Where(x => x.CreatedUser == user && x.FileType == fileType));
            Save();
        }

        /// <summary>
        /// Deletes document with specified identifier
        /// </summary>
        /// <param name="id">The document identifier</param>
        public void Delete(int id)
        {
            Set.RemoveRange(Set.Where(x => x.Id == id));
            Save();
        }

        /// <summary>
        /// Gets file for specified user and file type
        /// </summary>
        /// <param name="user">The user login</param>
        /// <param name="fileType">The file type</param>
        public AppDocument GetUserDocument(string user, string fileType)
        {
            return Set.FirstOrDefault(x => x.FileType == fileType && x.CreatedUser == user);
        }

        /// <summary>
        /// Gets the file specified by identifier
        /// </summary>
        /// <param name="id">The file id</param>
        public AppDocument GetDocument(int id)
        {
            return Set.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Check if the file exists for specified user and file type
        /// </summary>
        /// <param name="user">The user login</param>
        /// <param name="fileType">The file type</param>
        public bool IsExist(string user, string fileType)
        {
            return Set.Any(x => x.FileType == fileType && x.CreatedUser == user);
        }

        /// <summary>
        /// Check if the file exists for specified identifier
        /// </summary>
        /// <param name="id">The document identifier</param>
        public bool IsExist(int id)
        {
            return Set.Any(x => x.Id == id);
        }
    }
}
