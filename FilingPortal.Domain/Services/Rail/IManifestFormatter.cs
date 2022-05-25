using FilingPortal.Domain.DTOs.Rail.Manifest;

namespace FilingPortal.Domain.Services.Rail
{
    /// <summary>
    /// Rail manifest formatter
    /// </summary>
    public interface IManifestFormatter
    {
        /// <summary>
        /// Formats model to formatted text
        /// </summary>
        /// <param name="manifest">Rail manifest</param>
        string Format(Manifest manifest);
        /// <summary>
        /// Gets additional styles if any
        /// </summary>
        string GetManifestStyles();
    }
}