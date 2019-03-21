using System;

namespace EventStore.infrastructure.EventStores
{
    [Serializable]
    public class OptimsticConcurrencyException : Exception
    {

        public OptimsticConcurrencyException(string message) : base(message)
        {
        }
    }
}