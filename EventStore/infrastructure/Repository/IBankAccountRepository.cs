using System;
using EventStore.Domain.BankAccount;
using Shared;

namespace EventStore.infrastructure.Repository
{
    public interface IBankAccountRepository
    {
        Option<BankAccount> FindBy(Guid id);
        void Add(BankAccount bankAccount);
        void Save(BankAccount bankAccount);
        void SaveSnapshot(BankAccountSnapshot snapshot, BankAccount bankAccount);
    }
}