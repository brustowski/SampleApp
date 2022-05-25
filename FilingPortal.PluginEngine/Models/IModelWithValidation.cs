using System.Collections.Generic;

namespace FilingPortal.PluginEngine.Models
{
    public interface IModelWithValidation<TValidationObject>
    {
        /// <summary>
        /// Validation errors
        /// </summary>
        List<TValidationObject> Errors { get; set; }
    }
}
