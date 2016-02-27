namespace RockPaperScissors.Messages
{
    public class GameStarted : IGameEvent
    {
        public int GameId { get; set; }
    }
}