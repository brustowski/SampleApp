using System.Linq;
using FluentValidation;

namespace FilingPortal.Parts.Zones.Entry.Domain.Import.Xml.Inbound
{
    /// <summary>
    /// Provides validator for <see cref="CUSTOMS_ENTRY_FILEENTRY"/>
    /// </summary>
    internal class Validator : AbstractValidator<CUSTOMS_ENTRY_FILEENTRY>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        public Validator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.HEADER_ADDRESS)
                .Must(ContainsImporter).WithMessage("Importer not found");

            RuleFor(x => x.ENTRY_NO).MaximumLength(7);
            RuleFor(x => x.CHECK_DIGIT).MaximumLength(1);
        }

        private bool ContainsImporter(CUSTOMS_ENTRY_FILEENTRYHEADER_ADDRESS[] headerAddreses)
        {
            if (headerAddreses.Length == 0) return false;

            CUSTOMS_ENTRY_FILEENTRYHEADER_ADDRESS importer = headerAddreses.FirstOrDefault(x => x.TYPE == "IM");
            if (importer == null) return false;

            return !string.IsNullOrWhiteSpace(importer.MID_OR_IRS_NO);
        }
    }
}
