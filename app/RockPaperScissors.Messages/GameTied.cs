namespace RockPaperScissors.Messages
{
    public class GameTied : IEvent 
    {
        public int GameId { get; set; }
    }
}
