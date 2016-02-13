using System;
using NUnit.Framework;
using Rhino.Mocks;
using RockPaperScissors.Messages;
using SharpTestsEx;
using StructureMap;

namespace RockPaperScissors.Tasks.Infrastructure
{

    [TestFixture]
    public class ContainerContextShould
    {

        [Test]
        public void CreateANestedContainerWhenConstructed()
        {
            var nestedContainer = MockRepository.GenerateStrictMock<IContainer>();
            nestedContainer.Expect(c => c.Configure(Arg<Action<ConfigurationExpression>>.Is.NotNull))
                .Repeat.Once();

            var container = MockRepository.GenerateStrictMock<IContainer>();
            container.Expect(c => c.GetNestedContainer())
                .Return(nestedContainer)
                .Repeat.Once();

            var context = new ContainerContext(container);
            container.VerifyAllExpectations();
        }

        [Test]
        public void DisposeTheNestedContainerWhenDisposed()
        {
            var nestedContainer = MockRepository.GenerateStrictMock<IContainer>();
            nestedContainer.Expect(c => c.Configure(Arg<Action<ConfigurationExpression>>.Is.NotNull))
                .Repeat.Once();

            nestedContainer.Expect(c => c.Dispose())
                .Repeat.Once();

            var container = MockRepository.GenerateStrictMock<IContainer>();
            container.Expect(c => c.GetNestedContainer())
                .Return(nestedContainer)
                .Repeat.Once();

            using (var context = new ContainerContext(container))
                //NOOP
                ;

            nestedContainer.VerifyAllExpectations();
        }

        [Test]
        public void ReturnMediatorFromNestedContainer()
        {
            var nestedContainer = MockRepository.GenerateStrictMock<IContainer>();
            nestedContainer.Expect(c => c.Configure(Arg<Action<ConfigurationExpression>>.Is.NotNull))
                .Repeat.Once();

            var expected = MockRepository.GenerateStrictMock<IMediator>();

            nestedContainer.Expect(c => c.GetInstance<IMediator>())
                .Return(expected)
                .Repeat.Once();

            nestedContainer.Expect(c => c.Dispose())
                .Repeat.Once();

            var container = MockRepository.GenerateStrictMock<IContainer>();
            container.Expect(c => c.GetNestedContainer())
                .Return(nestedContainer)
                .Repeat.Once();

            IMediator actual;
            using (var context = new ContainerContext(container))
                actual = context.Mediator;

            nestedContainer.VerifyAllExpectations();
            actual.Should().Be.SameInstanceAs(expected);
        }

    }
}
