namespace RockPaperScissors.Messages
{
    public class GameTied : IGameEvent 
    {
        public int GameId { get; set; }
    }
}
