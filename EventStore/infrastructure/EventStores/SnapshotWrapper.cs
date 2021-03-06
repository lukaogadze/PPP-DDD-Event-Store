﻿using System;

namespace EventStore.infrastructure.EventStores
{
    public class SnapshotWrapper
    {
        public string StreamName { get; private set; }
        public Object Snapshot { get; private set; }
        public DateTimeOffset Created { get; private set; }

        public SnapshotWrapper(string streamName, object snapshot)
        {
            StreamName = streamName;
            Snapshot = snapshot;
            Created = DateTimeOffset.UtcNow;
        }
    }
}
