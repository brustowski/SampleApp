namespace FilingPortal.Domain.Validators
{
    /// <summary>
    /// Interface for single Record validation
    /// </summary>
    public interface ISingleRecordValidator<in TModel> : ISingleRecordTypedValidator<string, TModel>
    {

    }
}