using RockPaperScissors.Messages;
using StructureMap;

namespace RockPaperScissors.Tasks.Infrastructure
{
    internal class ContainerContext : IContainerContext
    {
        private readonly IContainer _container;

        public ContainerContext(IContainer container)
        {
            _container = container.GetNestedContainer();
            _container.Configure(c =>
            {
                c.For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => ctx.GetInstance);
                c.For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => ctx.GetAllInstances);
                c.For<IMediator>().Use<Mediator>();
            });
        }

        public void Dispose()
        {
            _container.Dispose();
        }

        public IMediator Mediator => _container.GetInstance<IMediator>();
    }
}
