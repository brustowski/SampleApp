using System;

namespace Framework.Domain.Events
{
    public class Event : IEvent
    {
        public DateTime TimeStamp { get; set; }
    }
}