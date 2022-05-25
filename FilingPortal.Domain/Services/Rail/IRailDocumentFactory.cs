using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;

namespace FilingPortal.Domain.Services.Rail
{
    /// <summary>
    /// Interface for Rail document creation service
    /// </summary>
    public interface IRailDocumentFactory : IDocumentFactory<RailDocument>
    {
        /// <summary>
        /// Creates the Rail document manifest from specified file model and with specified creator name
        /// </summary>
        /// <param name="file">The file model</param>
        /// <param name="creatorName">The creator name</param>
        RailDocument CreateManifest(BinaryFileModel file, string creatorName);
    }
}