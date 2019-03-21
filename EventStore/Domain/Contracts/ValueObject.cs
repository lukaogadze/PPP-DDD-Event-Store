using System.Collections.Generic;
using System.Linq;

namespace EventStore.Domain.Contracts
{
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            var otherValueObject = obj as ValueObject;
            return GetEqualityComponents().SequenceEqual(otherValueObject.GetEqualityComponents());
        }
       
        

        public override int GetHashCode()
        {
            return GetEqualityComponents().Aggregate(1, (current, obj) =>
            {
                unchecked
                {
                    return current * 39 + (obj?.GetHashCode() ?? 0);
                }
            });
        }
        

        public static bool operator ==(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return !(left == right);
        }
    }
}
