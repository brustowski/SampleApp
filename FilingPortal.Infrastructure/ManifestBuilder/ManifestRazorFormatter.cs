using FilingPortal.Domain.Services;
using FilingPortal.Domain.Services.Rail;
using RazorEngine;
using RazorEngine.Templating;
using System.IO;
using FilingPortal.Domain.DTOs.Rail.Manifest;

namespace FilingPortal.Infrastructure.ManifestBuilder
{
    /// <summary>
    /// Formats RailBDParsed record to printable format using Razor Engine
    /// </summary>
    internal class ManifestRazorFormatter : IManifestFormatter
    {
        private string TemplatePath { get; }

        /// <summary>
        /// Creates new instance of <see cref="ManifestRazorFormatter"/>
        /// </summary>
        /// <param name="templateProviderService">Templates path service</param>
        public ManifestRazorFormatter(ITemplatesProviderService templateProviderService)
        {
            TemplatePath = templateProviderService.GetTemplatesFolder();
        }

        /// <summary>
        /// Formats model to formatted text
        /// </summary>
        /// <param name="manifest">Rail manifest</param>
        public string Format(Manifest manifest)
        {
            var template = File.ReadAllText(TemplatePath + "/manifest.cshtml");
            var result =
                Engine.Razor.RunCompile(template, "templateKey", typeof(Manifest), manifest);

            return result;
        }

        /// <summary>
        /// Gets additional styles
        /// </summary>
        public string GetManifestStyles()
        {
            return File.ReadAllText(TemplatePath + "/manifest.css");
        }
    }
}