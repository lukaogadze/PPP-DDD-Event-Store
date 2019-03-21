using EventStore.Domain.Events;

namespace EventStore.infrastructure.EventStores
{
    public class EventStream
    {
        public string Id { get; private set; }
        public int Version { get; private set; }
        private EventStream() { }
        public EventStream(string id)
        {
            Id = id;
            Version = 0;
        }
        public EventWrapper RegisterEvent(DomainEvent @event)
        {
            Version++;
            return new EventWrapper(@event, Version, Id);
        }
    }
}
