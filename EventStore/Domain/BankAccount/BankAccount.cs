using System;
using EventStore.Domain.Contracts;
using EventStore.Domain.Events;

namespace EventStore.Domain.BankAccount
{
    public class BankAccount : EventSourcedAggregate
    {
        public Balance Balance { get; private set; }
        public int LastStoredEventNumber { get; }
        public BankAccount(Guid id, int lastStoredEventNumber) : base(id)
        {
            Balance = Balance.Empty;
            LastStoredEventNumber = lastStoredEventNumber;
        }

        public BankAccount(Guid id): this(id, 0)
        {
        }

        public BankAccount(BankAccountSnapshot snapshot, int lastStoredEventNumber) :base(snapshot.AggregateId)
        {
            Version = snapshot.Version;
            LastStoredEventNumber = lastStoredEventNumber;
            Balance = Balance.Create(snapshot.Balance);
        }

        public override void Apply(DomainEvent @event)
        {
            When((dynamic)@event);
            Version++;
        }
        
        public BankAccountSnapshot Snapshot() => new BankAccountSnapshot(Version, Balance.Value, this.Id);

        public void Withdraw(Money money)
        {
            if (!Balance.MoneyCanBeWithdrawn(money))
            {
                // Log or do something
            }
            else
            {
                Causes(new WithdrawnMoney(this.Id, money));
            }
            
        }

        public void Deposit(Money money)
        {
            Causes(new DepositedMoney(this.Id, money));
        }
        
        
        private void Causes(DomainEvent @event)
        {
            Changes.Add(@event);
            Apply(@event);
        }

        private void When(WithdrawnMoney withdrawnMoney)
        {
            Balance = Balance.Withdraw(withdrawnMoney.Amount);
        }
                

        private void When(DepositedMoney depositedMoney)
        {
            Balance = Balance.Deposit(depositedMoney.Amount);
        }
    }
}