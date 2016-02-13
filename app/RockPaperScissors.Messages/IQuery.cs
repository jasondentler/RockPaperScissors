namespace RockPaperScissors.Messages
{

    public interface IQuery : IMessage
    {
    }

    public interface IQuery<TResponse> : IQuery
    {
    }
}