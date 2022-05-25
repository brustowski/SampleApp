namespace FilingPortal.Domain.Services.GridExport.Formatters
{
    internal class BoolNullableFormatter : ValueFormatter<bool?>
    {
        protected override string Format(bool? value)
        {
            if (value == null) return string.Empty;
            return (bool) value ? "Yes" : "No";
        }
    }
}