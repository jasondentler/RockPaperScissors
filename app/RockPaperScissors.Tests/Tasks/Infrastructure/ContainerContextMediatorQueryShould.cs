using NUnit.Framework;
using Rhino.Mocks;
using RockPaperScissors.Messages;

namespace RockPaperScissors.Tasks.Infrastructure
{
    public class ContainerContextMediatorSendCreateCommandShould : ContainerContextMediatorShould
    {

        [Test]
        public void UseAContainerContext()
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

            Mediator.Send(new TestCreateCommand());
        }

        [Test]
        public void CallTheInnerMediatorSend()
        {
            var mocks = new MockRepository();
            var innerMediator = MockInnerMediator();

            var command = new TestCreateCommand();

            innerMediator
                .Expect(m => m.Send(command))
                .Return(0)
                .Repeat.Once();

            Mediator.Send(command);

            mocks.VerifyAll();
        }


    }
    public class ContainerContextMediatorQueryShould : ContainerContextMediatorShould
    {

        [Test]
        public void UseAContainerContext()
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

            Mediator.Query(new TestQuery());
        }

        [Test]
        public void CallTheInnerMediatorSend()
        {
            var mocks = new MockRepository();
            var innerMediator = MockInnerMediator();

            var query = new TestQuery();

            innerMediator
                .Expect(m => m.Query(query))
                .Return(new[] {0, 1, 2, 3, 4})
                .Repeat.Once();

            Mediator.Query(query);

            mocks.VerifyAll();
        }


    }
}