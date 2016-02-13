using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using RockPaperScissors.Messages;
using SharpTestsEx;

namespace RockPaperScissors.Tasks.Infrastructure
{

    [TestFixture]
    public class MediatorShould
    {
        private IDictionary<Type, object> _mocks;
        private MockRepository _mockRepository;

        private T CreateMock<T>()
        {
            return (T) CreateMock(typeof (T));
        }

        private object CreateMock(Type type)
        {
            if (_mocks.ContainsKey(type))
            {
                Console.WriteLine("Returning existing mock for " + type);
                return _mocks[type];
            }

            Console.WriteLine("Creating mock for " + type);
            var mock = _mockRepository.StrictMock(type);
            _mocks[type] = mock;
            return mock;
        }

        private IMediator _mediator;

        [SetUp]
        public void Setup()
        {
            _mocks = new Dictionary<Type, object>();
            _mockRepository = new MockRepository();
            _mediator = new Mediator(CreateMock);
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var mock in _mocks.Values)
                if (_mockRepository.IsInReplayMode(mock))
                    mock.VerifyAllExpectations();
            _mocks.Clear();
        }

        public class TestCommand : ICommand
        {
        }

        public class TestCreateCommand : ICreateCommand<int>
        {
        }

        public class TestQuery : IQuery<List<int>>
        {
        }

        [Test]
        public void ThrowWhenSingleInstanceFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Mediator(null));
        }

        [Test]
        public void ThrowWhenTheCommandIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mediator.Send((ICommand) null));
        }

        [Test]
        public void ThrowWhenTheCreateCommandIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mediator.Send((ICreateCommand<int>)null));
        }
        [Test]
        public void ThrowWhenTheQueryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mediator.Query((IQuery<int>)null));
        }

        [Test]
        public void CallTheCommandHandler()
        {
            var command = new TestCommand();
            var handler = CreateMock<ICommandHandler<TestCommand>>();
            handler.Expect(h => h.Handle(command)).Repeat.Once();

            _mockRepository.ReplayAll();
            _mediator.Send(command);
        }

        [Test]
        public void CallTheCreateCommandHandler()
        {
            var command = new TestCreateCommand();
            var expected = 2;
            var handler = CreateMock<ICommandHandler<TestCreateCommand, int>>();
            handler.Expect(h => h.Handle(command)).Return(expected).Repeat.Once();

            _mockRepository.ReplayAll();
            var actual = _mediator.Send(command);

            actual.Should().Be.EqualTo(expected);
        }

        [Test]
        public void CallTheQueryHandler()
        {
            var query = new TestQuery();
            var expected = new List<int>() {1, 2, 3, 4};
            var handler = CreateMock<IQueryHandler<TestQuery, List<int>>>();
            handler.Expect(h => h.Handle(query)).Return(expected).Repeat.Once();

            _mockRepository.ReplayAll();
            var actual = _mediator.Query(query);

            actual.Should().Be.SameInstanceAs(expected);
        }

    }
}
