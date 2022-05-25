namespace FilingPortal.Domain.Services.GridExport.Formatters
{
    internal abstract class ValueFormatter<TValue> : IValueFormatter
    {
        public string Format(object value)
        {
            return Format((TValue)value);
        }

        protected abstract string Format(TValue value);
    }
}