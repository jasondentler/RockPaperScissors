namespace RockPaperScissors.Messages
{
    public interface IMediator
    {
        void Send(ICommand command);
        TId Send<TId>(ICreateCommand<TId> command);
        TResult Query<TResult>(IQuery<TResult> query);
    }
}