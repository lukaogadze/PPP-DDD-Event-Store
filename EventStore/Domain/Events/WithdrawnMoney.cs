using System;
using EventStore.Domain.BankAccount;

namespace EventStore.Domain.Events
{
    public class WithdrawnMoney : DomainEvent
    {
        public Money Amount { get; }

        public WithdrawnMoney(Guid aggregateId, Money amount) : base(aggregateId)
        {
            Amount = amount;
        }
    }
}