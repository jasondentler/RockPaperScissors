using System;

namespace RockPaperScissors.Domain.GamePlay
{
    public class PlayerNotInTheGameException : ApplicationException
    {
        public PlayerRef Player { get; set; }

        public PlayerNotInTheGameException(PlayerRef player) : base("Player is not in the game")
        {
            Player = player;
        }
    }
}
