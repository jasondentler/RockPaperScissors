using NUnit.Framework;
using Rhino.Mocks;
using RockPaperScissors.Messages;

namespace RockPaperScissors.Tasks.Infrastructure
{
    [TestFixture]
    public class DomainEventsPublisherEventHandlerShould
    {

        public class TestEvent : IEvent
        {
        }

        [Test]
        public void CallTheInnerHandler()
        {
            var mockRepository = new MockRepository();
            var @event = new TestEvent();
            var eventHandler = mockRepository.StrictMock<IEventHandler<TestEvent>>();
            var publisher = mockRepository.DynamicMock<IDomainEventsPublisher>();
            var decorator = new DomainEventsPublisherEventHandler<TestEvent>(eventHandler, publisher);

            using (mockRepository.Record())
                eventHandler
                    .Expect(h => h.Handle(@event))
                    .Repeat.Once();

            using (mockRepository.Playback())
                decorator.Handle(@event);

            mockRepository.VerifyAll();
        }

        [Test]
        public void PublishThenClearTheDomainEvents()
        {
            var mockRepository = new MockRepository();
            var @event = new TestEvent();
            var eventHandler = mockRepository.DynamicMock<IEventHandler<TestEvent>>();
            var publisher = mockRepository.StrictMock<IDomainEventsPublisher>();
            var decorator = new DomainEventsPublisherEventHandler<TestEvent>(eventHandler, publisher);

            using (mockRepository.Record())
            using (mockRepository.Ordered())
            {
                eventHandler
                    .Expect(h => h.Handle(@event))
                    .Repeat.Once();

                publisher.Expect(p => p.Publish());
                publisher.Expect(p => p.Clear());
            }

            using (mockRepository.Playback())
                decorator.Handle(@event);

            mockRepository.VerifyAll();
        }

    }
}