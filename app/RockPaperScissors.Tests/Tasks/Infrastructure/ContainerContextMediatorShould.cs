using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using RockPaperScissors.Messages;
using StructureMap;

namespace RockPaperScissors.Tasks.Infrastructure
{
    [TestFixture]
    public abstract class ContainerContextMediatorShould
    {

        public class TestCommand : ICommand
        {
        }

        public class TestCommandHandler : ICommandHandler<TestCommand>
        {
            public void Handle(TestCommand command)
            {
            }
        }

        public class TestCreateCommand : ICreateCommand<int>
        {
        }

        public class TestCreateCommandHandler : ICommandHandler<TestCreateCommand, int>
        {
            public int Handle(TestCreateCommand command)
            {
                return int.MaxValue;
            }
        }

        public class TestQuery : IQuery<IEnumerable<int>>
        {
        }

        public class TestQueryHandler : IQueryHandler<TestQuery, IEnumerable<int>>
        {
            public IEnumerable<int> Handle(TestQuery query)
            {
                yield return int.MaxValue;
            }
        }

        protected ContainerContextMediator Mediator { get; private set; }

        protected IContainer Container { get; private set; }

        private MockRepository _mockRepository;
        private readonly IList<object> _mocks = new List<object>();

        protected T GenerateStrictMock<T>()
        {
            var mock = _mockRepository.StrictMock<T>();
            _mocks.Add(mock);
            return mock;
        }

        protected IMediator MockInnerMediator()
        {
            var containerContext = GenerateStrictMock<IContainerContext>();
            Container.Expect(c => c.GetInstance<IContainerContext>())
                .Return(containerContext)
                .Repeat.Once();

            var innerMediator = GenerateStrictMock<IMediator>();

            containerContext.Expect(c => c.Mediator)
                .Return(innerMediator)
                .Repeat.Once();

            containerContext.Expect(c => c.Dispose())
                .Repeat.Once();

            return innerMediator;
        }

        [SetUp]
        public virtual void Setup()
        {
            _mockRepository = new MockRepository();
            Container = GenerateStrictMock<IContainer>();
            Mediator = new ContainerContextMediator(Container);
        }

        [TearDown]
        public virtual void TearDown()
        {
            foreach (var mock in _mocks)
                if (_mockRepository.IsInReplayMode(mock))
                    mock.VerifyAllExpectations();
            _mocks.Clear();
        }

    }
}