using FilingPortal.Domain.Infrastructure.Helpers;

namespace FilingPortal.Domain.Services.GridExport.Formatters
{
    internal class DecimalNullableFormatter : ValueFormatter<decimal?>
    {
        protected override string Format(decimal? value)
        {
            return value?.ToInvariantCultureWithoutTrailingZeroesForView() ?? string.Empty;
        }
    }
}