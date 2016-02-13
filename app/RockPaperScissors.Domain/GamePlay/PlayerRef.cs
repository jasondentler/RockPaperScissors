using System;

namespace RockPaperScissors.Domain.GamePlay
{
    public class PlayerRef : IValueObject, IEquatable<PlayerRef>, IEquatable<Player>
    {

        protected PlayerRef()
        {
        }

        public PlayerRef(Player player)
        {
            PlayerId = player.Id;
        }

        public virtual int PlayerId { get; protected set; }

        public bool Equals(PlayerRef other)
        {
            if (ReferenceEquals(other, this)) return true;
            if (ReferenceEquals(other, null)) return false;
            return Equals(other.PlayerId, PlayerId);
        }

        public bool Equals(Player other)
        {
            if (ReferenceEquals(other, null)) return false;
            return Equals(other.Id, PlayerId);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PlayerRef);
        }

        public override int GetHashCode()
        {
            return PlayerId.GetHashCode();
        }

        public static implicit operator PlayerRef(Player player)
        {
            return ReferenceEquals(player, null) ? null : new PlayerRef(player);
        }
    }
}