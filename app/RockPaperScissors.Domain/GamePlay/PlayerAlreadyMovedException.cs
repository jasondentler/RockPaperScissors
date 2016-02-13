using System;

namespace RockPaperScissors.Domain.GamePlay
{
    public class PlayerAlreadyMovedException : ApplicationException
    {
        public PlayerRef Player { get; set; }

        public PlayerAlreadyMovedException(PlayerRef player) : base("Player already moved.")
        {
            Player = player;
        }
    }
}
