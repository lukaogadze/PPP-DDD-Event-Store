using System;
using System.Collections.Generic;
using EventStore.Domain.Contracts;

namespace EventStore.Domain.BankAccount
{
    public class Money : ValueObject
    {
        public decimal Value { get; private set; }

        public Money(decimal value)
        {
            if (value % 0.01m != 0)
                throw new InvalidOperationException("There cannot be more than two decimal places."); 

            if(value < 0)
                throw new InvalidOperationException("Money cannot be a negative value.");

            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}