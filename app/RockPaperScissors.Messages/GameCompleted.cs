namespace RockPaperScissors.Messages
{
    public class GameCompleted : IEvent 
    {
        public int GameId { get; set; }
    }
}
