namespace FilingPortal.Domain.Services.GridExport.Formatters
{
    public interface IValueFormatter
    {
        string Format(object value);
    }
}