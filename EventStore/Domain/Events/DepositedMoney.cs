using System;
using EventStore.Domain.BankAccount;

namespace EventStore.Domain.Events
{
    public class DepositedMoney : DomainEvent
    {
        public Money Amount { get; }

        public DepositedMoney(Guid aggregateId, Money amount) : base(aggregateId)
        {
            Amount = amount;
        }
    }
}