using System;
using RockPaperScissors.Messages;

namespace RockPaperScissors.Tasks.Infrastructure
{
    internal interface IContainerContext : IDisposable
    {
        IMediator Mediator { get; }
    }
}