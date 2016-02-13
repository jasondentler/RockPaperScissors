namespace RockPaperScissors.Messages
{
    public class GameWon : IEvent 
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
    }
}
