using FluentValidation;

namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Describes Factory for Parsing Data Model Validators
    /// </summary>
    public interface IParsingDataModelValidatorFactory
    {
        /// <summary>
        /// Creates validator for specified Parsing Data Model type
        /// </summary>
        /// <typeparam name="T">Parsing Data Model type</typeparam>
        IValidator<T> Create<T>() where T : IParsingDataModel;
    }
}
