using System;

namespace EventStore.Domain.BankAccount
{
    public class BankAccountSnapshot
    {
        public int Version { get; }
        public decimal Balance { get; }
        public Guid AggregateId { get; }

        public BankAccountSnapshot(int version, decimal balance, Guid aggregateId)
        {
            Version = version;
            Balance = balance;
            AggregateId = aggregateId;
        }
    }
}