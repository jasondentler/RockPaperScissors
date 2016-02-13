namespace RockPaperScissors.Messages
{
    public class GameStarted : IEvent
    {
        public int GameId { get; set; }
    }
}