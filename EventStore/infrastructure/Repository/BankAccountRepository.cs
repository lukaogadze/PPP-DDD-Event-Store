using System;
using System.Linq;
using EventStore.Domain.BankAccount;
using EventStore.Domain.Events;
using EventStore.infrastructure.EventStores;
using Shared;

namespace EventStore.infrastructure.Repository
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly IEventStore _eventStore;

        public BankAccountRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }
        
        
        public Option<BankAccount> FindBy(Guid id)
        {
            var streamName = StreamFor(id);

            var fromEventNumber = 0;
            const int toEventNumber = int.MaxValue;

            Option<BankAccountSnapshot> snapshot = _eventStore.GetLatestSnapshot<BankAccountSnapshot>(streamName);
            if (snapshot.IsSome)
            {
                fromEventNumber = snapshot.Value.Version + 1;
            }

            var eventStream = _eventStore.GetStream(streamName, fromEventNumber, toEventNumber);

            BankAccount bankAccount = null;
            if (snapshot.IsSome)
            {
                bankAccount = new BankAccount(snapshot.Value, null);
            }
            else
            {
                bankAccount = new BankAccount(id);
            }

            foreach (DomainEvent @event in eventStream)
            {
                bankAccount.Apply(@event);
            }
            
            return Option.Some(bankAccount);
        }
        
        

        public void Add(BankAccount bankAccount)
        {
            var streamName = StreamFor(bankAccount.Id);
            _eventStore.CreateNewStream(streamName, bankAccount.Changes);
        }

        public void Save(BankAccount bankAccount)
        {
            var streamName = StreamFor(bankAccount.Id);

            var expectedVersion = GetExpectedVersion(bankAccount.InitialVersion);
            _eventStore.AppendEventsToStream(streamName, bankAccount.Changes, expectedVersion);   
        }
        
        public void SaveSnapshot(BankAccountSnapshot snapshot, BankAccount bankAccount)
        {
            var streamName = StreamFor(bankAccount.Id);            

            _eventStore.AddSnapshot(streamName, snapshot);
        }



        private string StreamFor(Guid id) => $"{typeof(BankAccount).Name}-{id}";
        private Option<int> GetExpectedVersion(int expectedVersion) =>
            expectedVersion == 0 ? Option.None<int>() : Option.Some(expectedVersion);
    }
}