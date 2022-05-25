using System.Collections.Generic;

namespace FilingPortal.PluginEngine.Models
{
    /// <summary>
    /// Provides access to data that have been posted by a client 
    /// </summary>
    public class HttpPostedData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpPostedData"/> class
        /// </summary>
        /// <param name="fields">Set of fields that have been provided by a client</param>
        /// <param name="files">Set of files that have been provided by a client</param>
        public HttpPostedData(IDictionary<string, HttpPostedField> fields, IDictionary<string, HttpPostedFile> files)
        {
            Fields = fields;
            Files = files;
        }
        /// <summary>
        /// Gets or set fields that have been provided by a client
        /// </summary>
        public IDictionary<string, HttpPostedField> Fields { get; private set; }
        /// <summary>
        /// Gets or set files that have been provided by a client
        /// </summary>
        public IDictionary<string, HttpPostedFile> Files { get; private set; }
    }
}