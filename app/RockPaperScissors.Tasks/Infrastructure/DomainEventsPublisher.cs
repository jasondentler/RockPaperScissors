using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using RockPaperScissors.Domain;
using RockPaperScissors.Messages;

namespace RockPaperScissors.Tasks.Infrastructure
{

    internal class DomainEventsPublisher : IDomainEventsPublisher
    {
        private readonly MultiInstanceFactory _multiInstanceFactory;
        public DomainEventsPublisher(MultiInstanceFactory multiInstanceFactory)
        {
            if (multiInstanceFactory == null) throw new ArgumentNullException(nameof(multiInstanceFactory));
            _multiInstanceFactory = multiInstanceFactory;
        }

        public void Clear()
        {
            DomainEvents.Clear();
        }

        public void Publish()
        {
            foreach (var @event in DomainEvents.GetEvents())
                Publish(@event);
        }

        private static ConcurrentDictionary<Type, Action<DomainEventsPublisher, IEvent>> _eventDelegates = new ConcurrentDictionary<Type, Action<DomainEventsPublisher, IEvent>>();

        private void Publish(IEvent @event)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));
            var eventType = @event.GetType();
            var action = _eventDelegates.GetOrAdd(eventType, x =>
            {
                var publisherParam = Expression.Parameter(typeof(DomainEventsPublisher), "domainEventsPublisher");
                var eventParam = Expression.Parameter(typeof(IEvent), "event");
                var castOperation = Expression.Convert(eventParam, eventType);
                var call = Expression.Call(publisherParam,
                    "ExecutePublish", new[] { eventType },
                    castOperation);
                var lambda = Expression.Lambda<Action<DomainEventsPublisher, IEvent>>(call, publisherParam, eventParam);
                Console.WriteLine(lambda.ToString());
                return lambda.Compile();
            });
            action(this, @event);
        }

        private void ExecutePublish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var handlers = Create<IEventHandler<TEvent>>().ToArray();
            Console.WriteLine("Found {0} handlers for {1}", handlers.Length, typeof(TEvent));
            foreach (var handler in handlers)
                handler.Handle(@event);
        }

        private IEnumerable<T> Create<T>()
        {
            return _multiInstanceFactory(typeof (T)).Cast<T>();
        }

    }
}
