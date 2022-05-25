using System;
using System.Diagnostics;

namespace Framework.Infrastructure
{
    public class MonitoredScope : IDisposable
    {
        private readonly string scopeMessage;
        private Stopwatch stopwatch = new Stopwatch();

        public MonitoredScope(string scopeMessage)
        {
            this.scopeMessage = scopeMessage;
            stopwatch.Start();
            AppLogger.Trace($"Operation \"{scopeMessage}\" started");
        }

        public void Dispose()
        {
            stopwatch.Stop();
            AppLogger.Trace($"Operation \"{scopeMessage}\" finished in {stopwatch.ElapsedMilliseconds} ms.");
        }
    }
}
