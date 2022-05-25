namespace FilingPortal.PluginEngine.Services.Filing.Auto.AutoFilingProcessors
{
    /// <summary>
    /// Handles error in records processor
    /// </summary>
    /// <param name="record">Record</param>
    /// <param name="message">Message</param>
    public delegate void ErrorHandler<in TInboundType>(TInboundType record, string message);
}