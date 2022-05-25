using System;
using System.Collections.Generic;
using System.Linq;
using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.Models
{
    /// <summary>
    /// Class describing validation result model with errors of specific fields
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false)]
    public class ValidationResultWithFieldsErrorsViewModel : ValidationResultViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResultWithFieldsErrorsViewModel"/> class
        /// using the specified error message
        /// </summary>
        public ValidationResultWithFieldsErrorsViewModel(string errorMessage) : base(errorMessage)
        {
            FieldsErrors = new List<FieldErrorViewModel>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResultWithFieldsErrorsViewModel"/> class
        /// </summary>
        public ValidationResultWithFieldsErrorsViewModel()
        {
            FieldsErrors = new List<FieldErrorViewModel>();
        }
        
        /// <summary>
        /// Gets or sets the errors of specific fields
        /// </summary>
        public List<FieldErrorViewModel> FieldsErrors { get; protected set; }

        /// <summary>
        /// Determines whether there are no errors
        /// </summary>
        public override bool IsValid => String.IsNullOrEmpty(CommonError) && !FieldsErrors.Any();

        /// <summary>
        /// Adds the error message of the field with specified name
        /// </summary>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="errorMessage">The error message</param>
        [TsIgnore]
        public void AddFieldError(string fieldName, string errorMessage)
        {
            FieldsErrors.Add(new FieldErrorViewModel {FieldName = fieldName, Message = errorMessage});
        }

        /// <summary>
        /// Adds several error messages of the fields
        /// </summary>
        /// <param name="fieldErrors">The field errors</param>
        [TsIgnore]
        public void AddFieldErrors(IEnumerable<FieldErrorViewModel> fieldErrors)
        {
            FieldsErrors.AddRange(fieldErrors);
        }

        /// <summary>
        /// Sets the common error message
        /// </summary>
        /// <param name="message">The message</param>

        [TsIgnore]
        public void SetCommonError(string message)
        {
            CommonError = message;
        }

        /// <summary>
        /// Sets the common error message from string collection
        /// </summary>
        /// <param name="messages">Collection of messages</param>
        /// <param name="delimiter">Delimiter of messages</param>
        [TsIgnore]
        public void SetCommonError(IEnumerable<string> messages, string delimiter = "\r\n")
        {
            CommonError = string.Join(delimiter, messages);
        }
    }


    /// <summary>
    /// Class describing validation result model with errors of specific fields and additional data
    /// </summary>
    /// <typeparam name="T">Additional data type</typeparam>
    [TsInterface(IncludeNamespace = false, AutoI = false, Name = "ValidationResultWithFieldsErrorsViewModelTyped")]
    public class ValidationResultWithFieldsErrorsViewModel<T> : ValidationResultWithFieldsErrorsViewModel
    {
        /// <summary>
        /// Additional data
        /// </summary>
        public T Data { get; set; }
    }
}