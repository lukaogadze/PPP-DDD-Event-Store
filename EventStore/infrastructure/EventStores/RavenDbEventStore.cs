using System.Collections.Generic;
using System.Linq;
using EventStore.Domain.Events;
using Raven.Client.Documents.Session;
using Shared;

namespace EventStore.infrastructure.EventStores
{
    public class RavenDbEventStore : IEventStore
    {
        private readonly IDocumentSession _documentSession;
        public RavenDbEventStore(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public void AddSnapshot<T>(string streamName, T snapshot)
        {
            var wrapper = new SnapshotWrapper(streamName, snapshot);

            _documentSession.Store(wrapper);
        }


        public void CreateNewStream(string streamName, IEnumerable<DomainEvent> domainEvents)
        {
            var eventStream = new EventStream(streamName);
            _documentSession.Store(eventStream);

            AppendEventsToStream(streamName, domainEvents, Option.None<int>());
        }

        public void AppendEventsToStream(string streamName, IEnumerable<DomainEvent> domainEvents, Option<int> expectedVersion)
        {
            var stream = _documentSession.Load<EventStream>(streamName);

            if (expectedVersion.IsSome)
            {
                CheckForConcurrencyError(expectedVersion.Value, stream);
            }

            foreach (var @event in domainEvents)
            {
                _documentSession.Store(stream.RegisterEvent(@event));
            }
        }

        public Option<T> GetLatestSnapshot<T>(string streamName) where T : class
        {
            var lastSnapshotWrapper = _documentSession.Query<SnapshotWrapper>()
                .Customize(x => x.WaitForNonStaleResults())
                .Where(x => x.StreamName == streamName)
                .OrderByDescending(x => x.Created)
                .FirstOrDefault();

            if (lastSnapshotWrapper == null)
            {
                return Option.None<T>();
            }

            return Option.Some(lastSnapshotWrapper.Snapshot as T);
        }

        public Option<List<EventWrapper>> GetStream(string streamName, int fromVersion, int toVersion)
        {
            var eventWrappers = _documentSession.Query<EventWrapper>()
                .Customize(x => x.WaitForNonStaleResults())
                .Where(x => x.EventStreamId == streamName
                && x.EventNumber <= toVersion
                && x.EventNumber >= fromVersion)
                .OrderBy(x => x.EventNumber)
                .ToList();

            if (eventWrappers.Count == 0)
            {
                return Option.None<List<EventWrapper>>();
            }
            
            return Option.Some(eventWrappers);
        }

        private static void CheckForConcurrencyError(int expectedVersion, EventStream stream)
        {
            var lastUpdatedVersion = stream.Version;
            if (lastUpdatedVersion != expectedVersion)
            {
                var error = $"Expected: {expectedVersion}. Found: {lastUpdatedVersion}";
                throw new OptimsticConcurrencyException(error);
            }
        }
    }
}
