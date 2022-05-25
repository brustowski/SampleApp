using System.Threading;
using Framework.Domain.Events;

namespace FilingPortal.DataLayer
{
    //TODO Add implementation for EventBus
    class EventBusSyncWrapper : IEventBusSyncWrapper
    {
        public void Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = new CancellationToken())
            where TEvent : class, IEvent
        {
            //TODO Refactor sync/async in EventBusSyncWrapper
            //Task.Run(() => _eventBus.Publish(@event, cancellationToken), cancellationToken);
        }
    }
}