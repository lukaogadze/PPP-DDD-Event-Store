using System;
using EventStore.Domain;
using EventStore.Domain.BankAccount;
using EventStore.infrastructure.Repository;
using EventStore.infrastructure.EventStores;
using NUnit.Framework;
using Raven.Client.Documents;

namespace Tests.Demo
{
    [TestFixture]
    public class BankAccountShould
    {
        [Test]
        public void Get_Data_From_DB()
        {
            var guid = Guid.Parse("1cb52370-b00a-4a03-a7af-bec1e93e0476");
            
            using (var store = new DocumentStore
            {
                Urls = new[] {"http://localhost:8888"},
                Database = "BankAccountEventStore"
            })
            {
                store.Initialize();
                using (var session = store.OpenSession())
                {
                    IEventStore eventStore = new RavenDbEventStore(session);
                    var repository = new BankAccountRepository(eventStore);

                    var bankAccount = repository.FindBy(guid).Value;
                }
            }
        }
        
        
        
        [Test]
        public void DoWork()
        {
            var guid = Guid.NewGuid();
            var bankAccount = new BankAccount(guid);
            bankAccount.Deposit(new Money(100m));
            bankAccount.Deposit(new Money(150m));
            bankAccount.Withdraw(new Money(100m));

            using (var store = new DocumentStore
            {
                Urls = new[] {"http://localhost:8888"},
                Database = "BankAccountEventStore"
            })
            {
                store.Initialize();
                using (var session = store.OpenSession())
                {
                    IEventStore eventStore = new RavenDbEventStore(session);
                    var repository = new BankAccountRepository(eventStore);
                    repository.Add(bankAccount);
                    session.SaveChanges();


//                    repository.SaveSnapshot(bankAccount.Snapshot(), bankAccount);
//                    session.SaveChanges();
//                    
//                    var bankAccountOption = repository.FindBy(guid);
//                    var bankAccount2 = bankAccountOption.Value;
                    
                }
            }
        }
    }
}