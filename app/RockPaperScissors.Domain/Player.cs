using System;
using RockPaperScissors.Domain.GamePlay;

namespace RockPaperScissors.Domain
{
    public class Player : IAggregateRoot, IEquatable<Player>, IEquatable<PlayerRef>
    {
        public virtual int Id { get; set; }

        public string Name { get; set; }

        public bool Equals(Player other)
        {
            if (ReferenceEquals(other, this)) return true;
            if (ReferenceEquals(other, null)) return false;
            return Equals(other.Id, Id);
        }

        public bool Equals(PlayerRef other)
        {
            if (ReferenceEquals(other, null)) return false;
            return other.PlayerId == Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Player);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}