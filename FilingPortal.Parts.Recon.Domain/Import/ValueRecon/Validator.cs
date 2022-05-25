using FluentValidation;

namespace FilingPortal.Parts.Recon.Domain.Import.ValueRecon
{
    /// <summary>
    /// Provides validator for <see cref="Model"/>
    /// </summary>
    public class Validator : AbstractValidator<Model>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        public Validator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.EntryNo)
                .NotEmpty();
        }
    }
}
