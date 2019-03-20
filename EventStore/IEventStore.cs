using Shared;
using System;
using System.Collections.Generic;

namespace EventStore
{
    public interface IEventStore
    {
        void CreateNewStream(string streamName, IEnumerable<DomainEvent> domainEvents);
        void AppendEventsToStream(string streamName, IEnumerable<DomainEvent>domainEvents, Option<int> expectedVersion);
        Option<IEnumerable<DomainEvent>> GetStream(string streamName, int fromVersion, int toVersion);
        void AddSnapshot<T>(string streamName, T snapshot);
        Option<T> GetLatestSnapshot<T>(string streamName) where T : class;
    }
}
