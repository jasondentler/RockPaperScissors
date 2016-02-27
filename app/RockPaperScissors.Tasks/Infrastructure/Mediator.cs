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
            var action = _commandDelegates.GetOrAdd(commandType, GenerateCommandDelegate);
            action(this, command);
        }


        private Action<Mediator, ICommand> GenerateCommandDelegate(Type commandType)
        {
            var mediatorParam = Expression.Parameter(typeof(Mediator), "mediator");
            var commandParam = Expression.Parameter(typeof(ICommand), "command");
            var castOperation = Expression.Convert(commandParam, commandType);
            var action = new Action<ICommand>(ExecuteCommand);

            var mi = action.Method
                .GetGenericMethodDefinition()
                .MakeGenericMethod(commandType);

            var call = Expression.Call(mediatorParam, mi, castOperation);
            var lambda = Expression.Lambda<Action<Mediator, ICommand>>(call, mediatorParam, commandParam);
            return lambda.Compile();
        }

        public TId Send<TId>(ICreateCommand<TId> command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var commandType = command.GetType();
            var func = _createCommandDelegates.GetOrAdd(Tuple.Create(commandType, typeof (TId)), GenerateCommandDelegate);
            return (TId) func(this, command);
        }

        private Func<Mediator, ICommand, object> GenerateCommandDelegate(Tuple<Type, Type> io)
        {
            var commandType = io.Item1;
            var returnType = io.Item2;

            var mediatorParam = Expression.Parameter(typeof(Mediator), "mediator");
            var commandParam = Expression.Parameter(typeof(ICommand), "command");
            var castOperation = Expression.Convert(commandParam, commandType);

            var func = new Func<DummyCreateCommand, object>(ExecuteCreateCommand<DummyCreateCommand, object>);
            var mi = func.Method
                .GetGenericMethodDefinition()
                .MakeGenericMethod(commandType, returnType);

            var call = Expression.Call(mediatorParam, mi, castOperation);
            var castResult = Expression.Convert(call, typeof(object));
            var lambda = Expression.Lambda<Func<Mediator, ICommand, object>>(castResult, mediatorParam, commandParam);
            return lambda.Compile();
        }

        public TResult Query<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            var queryType = query.GetType();
            var func = _queryDelegates.GetOrAdd(Tuple.Create(queryType, typeof (TResult)), GenerateQueryDelegate);
            return (TResult) func(this, query);
        }

        private Func<Mediator, IQuery, object> GenerateQueryDelegate(Tuple<Type, Type> io)
        {
            var queryType = io.Item1;
            var returnType = io.Item2;

            var mediatorParam = Expression.Parameter(typeof(Mediator), "mediator");
            var queryParam = Expression.Parameter(typeof(IQuery), "query");
            var castOperation = Expression.Convert(queryParam, queryType);

            var func = new Func<DummyQuery, object>(ExecuteQuery<DummyQuery, object>);
            var mi = func.Method
                .GetGenericMethodDefinition()
                .MakeGenericMethod(queryType, returnType);

            var call = Expression.Call(mediatorParam, mi, castOperation);
            var castResult = Expression.Convert(call, typeof(object));
            var lambda = Expression.Lambda<Func<Mediator, IQuery, object>>(castResult, mediatorParam, queryParam);
            return lambda.Compile();
        }

        private void ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var handler = CreateInstance<ICommandHandler<TCommand>>();
            handler.Handle(command);
        }

        private TId ExecuteCreateCommand<TCommand, TId>(TCommand command) where TCommand : ICreateCommand<TId>
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var handler = CreateInstance<ICommandHandler<TCommand, TId>>();
            var id = handler.Handle(command);
            return id;
        }

        private TResult ExecuteQuery<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            var handler = CreateInstance<IQueryHandler<TQuery, TResult>>();
            var result = handler.Handle(query);
            return result;
        }

        private THandler CreateInstance<THandler>()
        {
            return (THandler) _singleInstanceFactory(typeof (THandler));
        }

        /// <remarks>
        /// For getting a compile-time reference to ExecuteCreateCommand inside GenerateCommandDelegate
        /// </remarks>
        private class DummyCreateCommand : ICreateCommand<object>
        {
        }

        /// <remarks>
        /// For getting a compile-time reference to ExecuteQuery inside GenerateQueryDelegate
        /// </remarks>
        private class DummyQuery : IQuery<object>
        {
        }

    }
}
