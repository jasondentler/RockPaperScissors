using System;

namespace RockPaperScissors.Domain.GamePlay
{
    public class InvalidGameStateException : ApplicationException
    {

        public InvalidGameStateException(GameStates gameStates) : base("Invalid action. Game is " + gameStates)
        {
        }

    }
}
