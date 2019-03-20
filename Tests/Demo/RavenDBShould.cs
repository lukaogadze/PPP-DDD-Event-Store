using NUnit.Framework;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Demo
{
    [TestFixture]
    public class RavenDBShould
    {
        [Test]
        public void Work()
        {
            using (var store = new DocumentStore
            {
                Urls = new string[] { "http://localhost:7979" },
                Database = "SampleDataDB"
            })
            {
                store.Initialize();

                using (var session = store.OpenSession())
                {
                    session.Store(new { Wat = "luka" });
                    session.SaveChanges();
                }
            }
        }
    }
}
