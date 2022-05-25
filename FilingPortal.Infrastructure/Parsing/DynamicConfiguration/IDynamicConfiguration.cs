namespace FilingPortal.Infrastructure.Parsing.DynamicConfiguration
{
    internal interface IDynamicConfiguration
    {
        DynamicMapInfo GetMapInfo(string internalFieldName);
    }
}
