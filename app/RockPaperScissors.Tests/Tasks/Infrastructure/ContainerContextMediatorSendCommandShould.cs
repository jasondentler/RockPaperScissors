using NUnit.Framework;
using Rhino.Mocks;
using RockPaperScissors.Messages;

namespace RockPaperScissors.Tasks.Infrastructure
{
    public class ContainerContextMediatorSendCommandShould : ContainerContextMediatorShould
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

            Mediator.Send(new TestCommand());
        }

        [Test]
        public void CallTheInnerMediatorSend()
        {
            var mocks = new MockRepository();
            var innerMediator = MockInnerMediator();

            var command = new TestCommand();
            innerMediator.Expect(m => m.Send(command))
                .Repeat.Once();
            Mediator.Send(command);

            mocks.VerifyAll();
        }


    }
}
