using System;

namespace RockPaperScissors.Domain.GamePlay
{
    public class InvalidPlayerCountException : ApplicationException
    {
        public int PlayerCount { get; private set; }

        public InvalidPlayerCountException(int playerCount)
            : base(string.Format("Unable to create a game with {0} players", playerCount))
        {
            PlayerCount = playerCount;
        }
    }
}
