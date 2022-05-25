using FilingPortal.Domain.DTOs;
using FilingPortal.Web.Models.Rail;

namespace FilingPortal.Web.FieldConfigurations
{
    /// <summary>
    /// Interface for Manifest factory
    /// </summary>
    public interface IManifestFactory
    {
        /// <summary>
        /// Creates manifest object
        /// </summary>
        /// <param name="manifest">Manifest data</param>
        ManifestModel CreateFrom(Manifest manifest);
    }
}
