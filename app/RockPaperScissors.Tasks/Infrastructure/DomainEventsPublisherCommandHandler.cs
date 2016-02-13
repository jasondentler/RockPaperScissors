using System;
using RockPaperScissors.Messages;

namespace RockPaperScissors.Tasks.Infrastructure
{
    internal class DomainEventsPublisherCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _inner;
        private readonly IDomainEventsPublisher _publisher;

        public DomainEventsPublisherCommandHandler(ICommandHandler<TCommand> inner, IDomainEventsPublisher publisher)
        {
            if (inner == null) throw new ArgumentNullException(nameof(inner));
            if (publisher == null) throw new ArgumentNullException(nameof(publisher));
            _inner = inner;
            _publisher = publisher;
        }

        public void Handle(TCommand command)
        {
            try
            {
                _inner.Handle(command);
                _publisher.Publish();
            }
            finally
            {
                _publisher.Clear();
            }
        }
    }

    internal class DomainEventsPublisherCommandHandler<TCommand, TId> : ICommandHandler<TCommand, TId> where TCommand : ICreateCommand<TId>
    {
        private readonly ICommandHandler<TCommand, TId> _inner;
        private readonly IDomainEventsPublisher _publisher;

        public DomainEventsPublisherCommandHandler(ICommandHandler<TCommand, TId> inner, IDomainEventsPublisher publisher)
        {
            if (inner == null) throw new ArgumentNullException(nameof(inner));
            if (publisher == null) throw new ArgumentNullException(nameof(publisher));
            _inner = inner;
            _publisher = publisher;
        }

        public TId Handle(TCommand command)
        {
            try
            {
                var result = _inner.Handle(command);
                _publisher.Publish();
                return result;
            }
            finally
            {
                _publisher.Clear();
            }
        }
    }
}