using System.Collections.Generic;
using EventStore.Domain.Events;
using Shared;

namespace EventStore.infrastructure.EventStores
{
    public interface IEventStore
    {
        void CreateNewStream(string streamName, IEnumerable<DomainEvent> domainEvents);
        void AppendEventsToStream(string streamName, IEnumerable<DomainEvent>domainEvents, Option<int> expectedVersion);
        IEnumerable<DomainEvent> GetStream(string streamName, int fromVersion, int toVersion);
        void AddSnapshot<T>(string streamName, T snapshot);
        Option<T> GetLatestSnapshot<T>(string streamName) where T : class;
    }
}
