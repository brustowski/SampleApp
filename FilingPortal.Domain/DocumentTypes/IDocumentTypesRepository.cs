using FilingPortal.Domain.DocumentTypes.Entities;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.DocumentTypes
{
    /// <summary>
    /// Interface for repository of <see cref="DocumentType"/>
    /// </summary>
    public interface IDocumentTypesRepository : IRepository<DocumentType>, IFilterDataRepository<DocumentType>
    {
        /// <summary>
        /// Gets document type by type name
        /// </summary>
        /// <param name="typeName">The type name</param>
        DocumentType GetByTypeName(string typeName);
    }
}
