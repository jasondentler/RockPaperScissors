namespace RockPaperScissors.Messages
{
    public class GameWon : IGameEvent 
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
    }
}
