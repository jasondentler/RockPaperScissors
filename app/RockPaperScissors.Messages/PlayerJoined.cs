namespace RockPaperScissors.Messages
{
    public class PlayerJoined : IPlayerEvent, IGameEvent
    {
        public int PlayerId { get; set; }
        public int GameId { get; set; }
    }
}
