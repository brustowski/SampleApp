namespace FilingPortal.Web.Models.Enums
{
    /// <summary>
    /// Filing types for different business processes
    /// </summary>
    public enum FilingType
    {
        /// <summary>
        /// Records should be filed separately with oun Filing Headers
        /// </summary>
        SingleFiling = 0,

        /// <summary>
        /// Records should be filed together with one filing header
        /// </summary>
        UnitTrade
    }
}
