namespace RockPaperScissors.Messages
{
    public interface IPlayerEvent : IEvent
    {
        int PlayerId { get; }
    }
}