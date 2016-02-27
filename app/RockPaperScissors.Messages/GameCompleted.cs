namespace RockPaperScissors.Messages
{
    public class GameCompleted : IGameEvent 
    {
        public int GameId { get; set; }
    }
}
