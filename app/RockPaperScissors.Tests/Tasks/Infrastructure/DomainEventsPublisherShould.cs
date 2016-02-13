using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using RockPaperScissors.Domain;
using RockPaperScissors.Messages;

namespace RockPaperScissors.Tasks.Infrastructure
{
    [TestFixture]
    public class DomainEventsPublisherShould
    {

        [SetUp]
        public void Setup()
        {
            DomainEvents.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            DomainEvents.Clear();
        }

        public class TestEvent : IEvent
        {
        }

        [Test]
        public void PublishDomainEvents()
        {
            var e = new TestEvent();

            var mockRepository = new MockRepository();
            IEnumerable<object> handlers;

            using (mockRepository.Record())
            {
                var handler1 = mockRepository.StrictMock<IEventHandler<TestEvent>>();
                handler1.Expect(h => h.Handle(e)).Repeat.Once();

                var handler2 = mockRepository.StrictMock<IEventHandler<TestEvent>>();
                handler2.Expect(h => h.Handle(e)).Repeat.Once();

                handlers = new[] { handler1, handler2 };
            }

            MultiInstanceFactory multiInstanceFactory = t => handlers;

            var publisher = new DomainEventsPublisher(multiInstanceFactory);
            DomainEvents.Raise(e);

            using (mockRepository.Playback())
                publisher.Publish();

            mockRepository.VerifyAll();
        }

        [Test]
        public void NotPublishClearedEvent()
        {
            var e = new TestEvent();

            var mockRepository = new MockRepository();
            IEnumerable<object> handlers;

            using (mockRepository.Record())
            {
                var handler1 = mockRepository.StrictMock<IEventHandler<TestEvent>>();
                handler1.Expect(h => h.Handle(e)).Repeat.Never();

                var handler2 = mockRepository.StrictMock<IEventHandler<TestEvent>>();
                handler2.Expect(h => h.Handle(e)).Repeat.Never();

                handlers = new[] { handler1, handler2 };
            }

            MultiInstanceFactory multiInstanceFactory = t => handlers;

            var publisher = new DomainEventsPublisher(multiInstanceFactory);
            DomainEvents.Raise(e);

            using (mockRepository.Playback())
            {
                publisher.Clear();
                publisher.Publish();
            }

            mockRepository.VerifyAll();
        }

    }
}
