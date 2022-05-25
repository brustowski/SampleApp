using System;
using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.Models
{
    /// <summary>
    /// Class describing validation result model
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false)]
    public class ValidationResultViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResultViewModel"/> class
        /// using the specified error message
        /// </summary>
        public ValidationResultViewModel(string errorMessage)
        {
            CommonError = errorMessage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResultViewModel"/> class
        /// </summary>
        public ValidationResultViewModel() { }

        /// <summary>
        /// Determines whether there are no errors
        /// </summary>
        public virtual bool IsValid => String.IsNullOrEmpty(CommonError);

        /// <summary>
        /// Gets the error message text
        /// </summary>
        public string CommonError { get; protected set; }
    }
}