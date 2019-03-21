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
            var guid = Guid.Parse("f63469c1-9caa-4a17-b7f5-3c8248193de9");
            
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

                    var bankAccount = repository.FindBy(guid);
                }
            }
        }
        
        
        
        [Test]
        public void DoWork()
        {
            var guid = Guid.Parse("f63469c1-9caa-4a17-b7f5-3c8248193de9");

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
                    var bankAccount = repository.FindBy(guid);

                    repository.SaveSnapshot(bankAccount.Snapshot(), bankAccount);

                    bankAccount.Deposit(new Money(200m));
                    repository.Save(bankAccount);


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