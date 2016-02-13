namespace RockPaperScissors.Tasks.Infrastructure
{
    internal interface IDomainEventsPublisher
    {
        void Clear();
        void Publish();
    }
}