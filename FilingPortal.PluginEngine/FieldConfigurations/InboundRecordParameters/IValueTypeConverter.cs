namespace FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters
{
    /// <summary>
    /// Interface for converter from db value type to ui value type
    /// </summary>
    public interface IValueTypeConverter
    {
        /// <summary>
        /// Converts the specified db value type into ui value type
        /// </summary>
        /// <param name="valueType">Db type of the value</param>
        string Convert(string valueType);
    }
}