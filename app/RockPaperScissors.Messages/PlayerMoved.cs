namespace RockPaperScissors.Messages
{
    public class PlayerMoved : IEvent 
    {

        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public Moves Move { get; set; }

    }
}
