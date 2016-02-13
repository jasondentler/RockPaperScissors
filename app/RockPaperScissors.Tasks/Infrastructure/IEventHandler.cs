using RockPaperScissors.Messages;

namespace RockPaperScissors.Tasks.Infrastructure
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        void Handle(TEvent evt);
    }
}