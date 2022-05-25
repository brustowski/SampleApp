using System.Linq;
using FluentValidation;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Import.Xml.Inbound
{
    /// <summary>
    /// Provides validator for <see cref="FTZ_214FTZ_ADMISSION"/>
    /// </summary>
    internal class Validator : AbstractValidator<FTZ_214FTZ_ADMISSION>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        public Validator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
        }
    }
}
