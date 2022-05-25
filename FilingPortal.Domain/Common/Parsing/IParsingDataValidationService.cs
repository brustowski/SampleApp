using Framework.Domain;
using System.Collections.Generic;

namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Describes parsing data validation service with specified parsing data model
    /// </summary>
    public interface IParsingDataValidationService<TParsingModel> 
        where TParsingModel : IParsingDataModel
    {
        /// <summary>
        /// Validates parsed data
        /// </summary>
        /// <typeparam name="TParsingModel">Parsing data model type</typeparam>
        /// <param name="records">Collection of data to validate</param>
        ParsedDataValidationResult<TParsingModel> Validate(IEnumerable<TParsingModel> records);
    }
}
