using System.Collections.Generic;

namespace RockPaperScissors.Messages
{
    public static class TestMessageTypes
    {

        public class Command : ICommand
        {
        }

        public class Event : IEvent
        {
        }

        public class Query : IQuery<IEnumerable<int>>
        {
        }

        public class Message : IMessage
        {
        }

        public class NotAMessage
        {
        }
    }
}