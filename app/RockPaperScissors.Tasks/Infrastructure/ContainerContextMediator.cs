using RockPaperScissors.Messages;
using StructureMap;

namespace RockPaperScissors.Tasks.Infrastructure
{
    public class ContainerContextMediator : IMediator
    {
        private readonly IContainer _container;

        public ContainerContextMediator(IContainer container)
        {
            _container = container;
        }

        public void Send(ICommand command)
        {
            using (var context = _container.GetInstance<IContainerContext>())
                context.Mediator.Send(command);
        }

        public TId Send<TId>(ICreateCommand<TId> command)
        {
            using (var context = _container.GetInstance<IContainerContext>())
                return context.Mediator.Send(command);
        }

        public TResult Query<TResult>(IQuery<TResult> query)
        {
            using (var context = _container.GetInstance<IContainerContext>())
                return context.Mediator.Query(query);
        }

    }
}