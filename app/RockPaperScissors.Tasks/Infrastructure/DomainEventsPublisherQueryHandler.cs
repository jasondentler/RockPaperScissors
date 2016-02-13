using System;
using RockPaperScissors.Messages;

namespace RockPaperScissors.Tasks.Infrastructure
{
    internal class DomainEventsPublisherQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _inner;
        private readonly IDomainEventsPublisher _publisher;

        public DomainEventsPublisherQueryHandler(IQueryHandler<TQuery, TResult> inner, IDomainEventsPublisher publisher)
        {
            if (inner == null) throw new ArgumentNullException(nameof(inner));
            if (publisher == null) throw new ArgumentNullException(nameof(publisher));
            _inner = inner;
            _publisher = publisher;
        }

        public TResult Handle(TQuery query)
        {
            try
            {
                var result = _inner.Handle(query);
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