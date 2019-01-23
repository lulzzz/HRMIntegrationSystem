using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Domain
{
    public abstract class EntityBase<TEntity, TId> : IEquatable<EntityBase<TEntity, TId>> where TEntity : EntityBase<TEntity, TId>
    {
        [Column("Id")]
        public virtual TId Id { get; set; }

        public bool Equals(EntityBase<TEntity, TId> other)
        {
            if (other == null)
            {
                return false;
            }

            // Handle the case of comparing two NEW objects
            var otherIsTransient = Equals(other.Id, default(TId));
            var currentIsTransient = Equals(Id, default(TId));

            if (otherIsTransient && currentIsTransient)
            {
                return ReferenceEquals(other, this);
            }

            return other.Id.Equals(Id);
        }

        public override bool Equals(object obj)
        {
            var other = obj as TEntity;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            var thisIsTransient = Equals(Id, default(TId));

            // When this instance is transient, we use the base GetHashCode()
            return thisIsTransient ? base.GetHashCode() : Id.GetHashCode();
        }

        public static bool operator ==(EntityBase<TEntity, TId> x, EntityBase<TEntity, TId> y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(EntityBase<TEntity, TId> x, EntityBase<TEntity, TId> y)
        {
            return !(x == y);
        }
    }
}
