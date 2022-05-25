namespace FilingPortal.Domain.Tests.Services.GridExport.Formatters
{
    enum TestEnum
    {
        [System.ComponentModel.Description("Description1")]
        Value1 = 0,
        Value2
    }

    enum NotIncludedTestEnum
    {
        [System.ComponentModel.Description("Description")]
        Value = 0
    }
}
