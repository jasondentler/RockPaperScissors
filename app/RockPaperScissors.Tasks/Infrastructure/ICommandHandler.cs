using RockPaperScissors.Messages;

namespace RockPaperScissors.Tasks.Infrastructure
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        void Handle(TCommand command);
    }

    public interface ICommandHandler<in TCommand, out TId> where TCommand : ICreateCommand<TId>
    {
        TId Handle(TCommand command);
    }
}