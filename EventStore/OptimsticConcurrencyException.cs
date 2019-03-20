using System;
using System.Runtime.Serialization;

namespace EventStore
{
    [Serializable]
    public class OptimsticConcurrencyException : Exception
    {

        public OptimsticConcurrencyException(string message) : base(message)
        {
        }
    }
}