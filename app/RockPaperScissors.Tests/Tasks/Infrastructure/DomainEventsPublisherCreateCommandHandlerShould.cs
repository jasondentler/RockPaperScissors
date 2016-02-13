using NUnit.Framework;
using Rhino.Mocks;
using RockPaperScissors.Messages;

namespace RockPaperScissors.Tasks.Infrastructure
{
    [TestFixture]
    public class DomainEventsPublisherCreateCommandHandlerShould
    {

        public class TestCreateCommand : ICreateCommand<int>
        {
        }

        [Test]
        public void CallTheInnerHandler()
        {
            var mockRepository = new MockRepository();
            var command = new TestCreateCommand();
            var commandHandler = mockRepository.StrictMock<ICommandHandler<TestCreateCommand, int>>();
            var publisher = mockRepository.DynamicMock<IDomainEventsPublisher>();
            var decorator = new DomainEventsPublisherCommandHandler<TestCreateCommand, int>(commandHandler, publisher);

            using (mockRepository.Record())
                commandHandler
                    .Expect(h => h.Handle(command))
                    .Return(2)
                    .Repeat.Once();

            using (mockRepository.Playback())
                decorator.Handle(command);

            mockRepository.VerifyAll();
        }

        [Test]
        public void PublishThenClearTheDomainEvents()
        {
            var mockRepository = new MockRepository();
            var command = new TestCreateCommand();
            var commandHandler = mockRepository.DynamicMock<ICommandHandler<TestCreateCommand, int>>();
            var publisher = mockRepository.StrictMock<IDomainEventsPublisher>();
            var decorator = new DomainEventsPublisherCommandHandler<TestCreateCommand, int>(commandHandler, publisher);

            using (mockRepository.Record())
            using (mockRepository.Ordered())
            {
                commandHandler
                    .Expect(h => h.Handle(command))
                    .Return(2)
                    .Repeat.Once();

                publisher.Expect(p => p.Publish());
                publisher.Expect(p => p.Clear());
            }

            using (mockRepository.Playback())
                decorator.Handle(command);

            mockRepository.VerifyAll();
        }

    }
    [TestFixture]
    public class DomainEventsPublisherQueryHandlerShould
    {

        public class TestQuery : IQuery<int>
        {
        }

        [Test]
        public void CallTheInnerHandler()
        {
            var mockRepository = new MockRepository();
            var query = new TestQuery();
            var queryHandler = mockRepository.StrictMock<IQueryHandler<TestQuery, int>>();
            var publisher = mockRepository.DynamicMock<IDomainEventsPublisher>();
            var decorator = new DomainEventsPublisherQueryHandler<TestQuery, int>(queryHandler, publisher);

            using (mockRepository.Record())
                queryHandler
                    .Expect(h => h.Handle(query))
                    .Return(2)
                    .Repeat.Once();

            using (mockRepository.Playback())
                decorator.Handle(query);

            mockRepository.VerifyAll();
        }

        [Test]
        public void PublishThenClearTheDomainEvents()
        {
            var mockRepository = new MockRepository();
            var query = new TestQuery();
            var queryHandler = mockRepository.DynamicMock<IQueryHandler<TestQuery, int>>();
            var publisher = mockRepository.StrictMock<IDomainEventsPublisher>();
            var decorator = new DomainEventsPublisherQueryHandler<TestQuery, int>(queryHandler, publisher);

            using (mockRepository.Record())
            using (mockRepository.Ordered())
            {
                queryHandler
                    .Expect(h => h.Handle(query))
                    .Return(2)
                    .Repeat.Once();

                publisher.Expect(p => p.Publish());
                publisher.Expect(p => p.Clear());
            }

            using (mockRepository.Playback())
                decorator.Handle(query);

            mockRepository.VerifyAll();
        }

    }
}