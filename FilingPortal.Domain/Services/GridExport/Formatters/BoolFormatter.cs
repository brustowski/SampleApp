namespace FilingPortal.Domain.Services.GridExport.Formatters
{
    internal class BoolFormatter : ValueFormatter<bool>
    {
        protected override string Format(bool value)
        {
            return value ? "Yes" : "No";
        }
    }
}