using RockPaperScissors.Messages;

namespace RockPaperScissors.Tasks.Infrastructure
{
    public interface IQueryHandler<in TQuery, out TResult> where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery query);
    }
}