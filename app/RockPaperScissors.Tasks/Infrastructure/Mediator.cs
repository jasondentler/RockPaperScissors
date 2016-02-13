using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using RockPaperScissors.Messages;

namespace RockPaperScissors.Tasks.Infrastructure
{
    public class Mediator : IMediator
    {
        private readonly SingleInstanceFactory _singleInstanceFactory;

        public Mediator(SingleInstanceFactory singleInstanceFactory)
        {
            if (singleInstanceFactory == null) throw new ArgumentNullException(nameof(singleInstanceFactory));
            _singleInstanceFactory = singleInstanceFactory;
        }

        private static ConcurrentDictionary<Type, Action<Mediator, ICommand>> _commandDelegates = new ConcurrentDictionary<Type, Action<Mediator, ICommand>>();
        private static ConcurrentDictionary<Tuple<Type, Type>, Func<Mediator, ICommand, object>> _createCommandDelegates = new ConcurrentDictionary<Tuple<Type, Type>, Func<Mediator, ICommand, object>>();  
        private static ConcurrentDictionary<Tuple<Type, Type>, Func<Mediator, IQuery, object>> _queryDelegates = new ConcurrentDictionary<Tuple<Type, Type>, Func<Mediator, IQuery, object>>(); 

        public void Send(ICommand command) 
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var commandType = command.GetType();
            var action = _commandDelegates.GetOrAdd(commandType, x =>
            {
                var mediatorParam = Expression.Parameter(typeof (Mediator), "mediator");
                var commandParam = Expression.Parameter(typeof (ICommand), "command");
                var castOperation = Expression.Convert(commandParam, commandType);
                var call = Expression.Call(mediatorParam,
                    "ExecuteCommand", new[] {commandType},
                    castOperation);
                var lambda = Expression.Lambda<Action<Mediator, ICommand>>(call, mediatorParam, commandParam);
                Console.WriteLine(lambda.ToString());
                return lambda.Compile();
            });
            action(this, command);
        }


        public TId Send<TId>(ICreateCommand<TId> command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var commandType = command.GetType();
            var func = _createCommandDelegates.GetOrAdd(Tuple.Create(commandType, typeof (TId)), x =>
            {
                var mediatorParam = Expression.Parameter(typeof(Mediator), "mediator");
                var commandParam = Expression.Parameter(typeof(ICommand), "command");
                var castOperation = Expression.Convert(commandParam, commandType);
                var call = Expression.Call(mediatorParam,
                    "ExecuteCreateCommand", new[] { commandType, typeof(TId) },
                    castOperation);
                var castResult = Expression.Convert(call, typeof (object));
                var lambda = Expression.Lambda<Func<Mediator, ICommand, object>>(castResult, mediatorParam, commandParam);
                Console.WriteLine(lambda.ToString());
                return lambda.Compile();
            });
            return (TId) func(this, command);
        }

        public TResult Query<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            var queryType = query.GetType();
            var func = _queryDelegates.GetOrAdd(Tuple.Create(queryType, typeof (TResult)), x =>
            {
                var mediatorParam = Expression.Parameter(typeof(Mediator), "mediator");
                var queryParam = Expression.Parameter(typeof(IQuery), "query");
                var castOperation = Expression.Convert(queryParam, queryType);
                var call = Expression.Call(mediatorParam,
                    "ExecuteQuery", new[] { queryType, typeof(TResult) },
                    castOperation);
                var boxingOperation = Expression.Convert(call, typeof (object));
                var lambda = Expression.Lambda<Func<Mediator, IQuery, object>>(boxingOperation, mediatorParam, queryParam);
                Console.WriteLine(lambda.ToString());
                return lambda.Compile();
            });
            return (TResult) func(this, query);
        }

        private void ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            Console.WriteLine("Executing command " + typeof(TCommand));
            if (command == null) throw new ArgumentNullException(nameof(command));
            var handler = CreateInstance<ICommandHandler<TCommand>>();
            handler.Handle(command);
        }

        private TId ExecuteCreateCommand<TCommand, TId>(TCommand command) where TCommand : ICreateCommand<TId>
        {
            Console.WriteLine("Executing command " + typeof(TCommand));
            if (command == null) throw new ArgumentNullException(nameof(command));
            var handler = CreateInstance<ICommandHandler<TCommand, TId>>();
            var id = handler.Handle(command);
            return id;
        }

        private TResult ExecuteQuery<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            Console.WriteLine("Executing query " + typeof(TQuery));
            if (query == null) throw new ArgumentNullException(nameof(query));
            var handler = CreateInstance<IQueryHandler<TQuery, TResult>>();
            var result = handler.Handle(query);
            return result;
        }

        private THandler CreateInstance<THandler>()
        {
            return (THandler) _singleInstanceFactory(typeof (THandler));
        }

    }
}
