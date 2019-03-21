using System;

namespace EventStore.Domain.Contracts
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>>
    {
        public TId Id { get; }
        
        public Entity(TId id)
        {            
            if (Object.Equals(id, default(TId)))
            {
                throw new ArgumentException("The ID cannot be the default value.", nameof(id));
            }
            Id = id;
        }        

        public override bool Equals(object obj)
        {
            if (obj is Entity<TId> entity)
            {
                return this.Equals(entity);
            }
            return false;
        }

        
        public override int GetHashCode()
        {
            unchecked
            {
                return this.Id.GetHashCode();
            }
        }

        public bool Equals(Entity<TId> other)
        {
            return  !ReferenceEquals(other, null) && this.Id.Equals(other.Id);
        }

        public static bool operator ==(Entity<TId> entity1, Entity<TId> entity2)
        {
            if (ReferenceEquals(entity1, null) && ReferenceEquals(entity2, null))
            {
                return true;
            }

            if (ReferenceEquals(entity1, null) || ReferenceEquals(entity2, null))
            {
                return false;
            }

            return entity1.Equals(entity2);
        }

        public static bool operator !=(Entity<TId> entity1, Entity<TId> entity2)
        {
            return !(entity1 == entity2);
        }
    }
}
