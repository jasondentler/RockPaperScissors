using RockPaperScissors.Messages;

namespace RockPaperScissors.Tasks.Infrastructure
{
    internal class DomainEventsPublisherEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : IEvent
    {
        private readonly IEventHandler<TEvent> _inner;
        private readonly IDomainEventsPublisher _publisher;

        public DomainEventsPublisherEventHandler(IEventHandler<TEvent> inner, IDomainEventsPublisher publisher)
        {
            _inner = inner;
            _publisher = publisher;
        }

        public void Handle(TEvent evt)
        {
            try
            {
                _inner.Handle(evt);
                _publisher.Publish();
            }
            finally
            {
                _publisher.Clear();
            }
        }
    }
}