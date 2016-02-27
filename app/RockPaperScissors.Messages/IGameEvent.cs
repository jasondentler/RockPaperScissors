namespace RockPaperScissors.Messages
{
    public interface IGameEvent : IEvent
    {
        int GameId { get; }
    }
}