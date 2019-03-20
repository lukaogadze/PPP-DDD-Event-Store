using System;

namespace EventStore
{
    public abstract class DomainEvent
    {
        public DomainEvent(Guid aggregateId)
        {
            Id = aggregateId;
            OccurredOn = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset OccurredOn { get; private set; }

        public Guid Id { get; private set; }
    }
}