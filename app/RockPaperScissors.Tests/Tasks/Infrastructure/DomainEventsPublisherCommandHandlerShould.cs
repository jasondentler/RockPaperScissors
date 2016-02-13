using NUnit.Framework;
using Rhino.Mocks;
using RockPaperScissors.Messages;

namespace RockPaperScissors.Tasks.Infrastructure
{
    [TestFixture]
    public class DomainEventsPublisherCommandHandlerShould
    {

        public class TestCommand : ICommand
        {
        }

        [Test]
        public void CallTheInnerHandler()
        {
            var mockRepository = new MockRepository();
            var command = new TestCommand();
            var commandHandler = mockRepository.StrictMock<ICommandHandler<TestCommand>>();
            var publisher = mockRepository.DynamicMock<IDomainEventsPublisher>();
            var decorator = new DomainEventsPublisherCommandHandler<TestCommand>(commandHandler, publisher);

            using (mockRepository.Record())
                commandHandler
                    .Expect(h => h.Handle(command))
                    .Repeat.Once();

            using (mockRepository.Playback())
                decorator.Handle(command);

            mockRepository.VerifyAll();
        }

        [Test]
        public void PublishThenClearTheDomainEvents()
        {
            var mockRepository = new MockRepository();
            var command = new TestCommand();
            var commandHandler = mockRepository.DynamicMock<ICommandHandler<TestCommand>>();
            var publisher = mockRepository.StrictMock<IDomainEventsPublisher>();
            var decorator = new DomainEventsPublisherCommandHandler<TestCommand>(commandHandler, publisher);

            using (mockRepository.Record())
            using (mockRepository.Ordered())
            {
                commandHandler
                    .Expect(h => h.Handle(command))
                    .Repeat.Once();

                publisher.Expect(p => p.Publish());
                publisher.Expect(p => p.Clear());
            }

            using (mockRepository.Playback())
                decorator.Handle(command);

            mockRepository.VerifyAll();
        }

    }
}
