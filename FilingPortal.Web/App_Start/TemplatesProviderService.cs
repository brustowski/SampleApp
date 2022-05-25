using System;
using System.IO;
using System.Web.Hosting;
using FilingPortal.Domain.Services;

namespace FilingPortal.Web
{
    /// <summary>
    /// Provides information about templates stored in application
    /// </summary>
    public class TemplatesProviderService: ITemplatesProviderService
    {
        /// <summary>
        /// Provider templates full path
        /// </summary>
        public string GetTemplatesFolder()
        {
            return HostingEnvironment.MapPath("~/App_Data/Templates/Razor");
        }

        /// <summary>
        /// Provides specified template as an array of bytes
        /// </summary>
        /// <param name="templateName">The name of the template</param>
        public byte[] GetTemplateAsByteArray(string templateName)
        {
            var templateFolder = HostingEnvironment.MapPath("~/App_Data/Templates");
            var path = Path.Combine(templateFolder, templateName);
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            throw new Exception();
        }
    }
}