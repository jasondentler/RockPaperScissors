using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RockPaperScissors.Messages;

namespace RockPaperScissors.Domain
{
    public static class DomainEvents
    {
        private static readonly ThreadLocal<Queue<IEvent>> Events = new ThreadLocal<Queue<IEvent>>(() => new Queue<IEvent>());

        //Raises the given domain event
        public static void Raise<T>(T @event) where T : IEvent
        {
            Events.Value.Enqueue(@event);
        }

        public static IEnumerable<IEvent> GetEvents()
        {
            var queue = Events.Value;
            while (queue.Any())
                yield return queue.Dequeue();
        }

        public static void Clear()
        {
            Events.Value.Clear();
        }
    }
}
