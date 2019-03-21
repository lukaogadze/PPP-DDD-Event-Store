using System;
using System.Collections.Generic;
using EventStore.Domain.Events;

namespace EventStore.Domain.Contracts
{
    public abstract class EventSourcedAggregate : Entity<Guid>
    {
        public abstract void Apply(DomainEvent @event);
        public List<DomainEvent> Changes { get;}
        public int Version { get; protected set; }
        
        protected EventSourcedAggregate(Guid id) : base(id)
        {
            Changes = new List<DomainEvent>();
        }
    }
}