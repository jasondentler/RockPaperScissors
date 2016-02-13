using System;

namespace RockPaperScissors.Messages
{

    /// <summary>
    /// Conventions for RockPaperScissors.Messages message types
    /// </summary>
    /// <remarks>
    /// Use these extension methods to wire up NServiceBus unobtrusive messaging for this assembly
    /// </remarks>
    public static class MessageConventions
    {
        public static bool IsMessage(this Type type)
        {
            return typeof(IMessage).IsAssignableFrom(type);
        }

        public static bool IsNServiceBusMessage(this Type type)
        {
            return type.IsMessage();
        }

        public static bool IsCommand(this Type type)
        {
            return typeof(ICommand).IsAssignableFrom(type);
        }

        public static bool IsQuery(this Type type)
        {
            return typeof (IQuery).IsAssignableFrom(type);
        }

        public static bool IsNServiceBusCommand(this Type type)
        {
            // From the standpoint of NSB, a query is just a command with a return message
            return type.IsCommand() || type.IsQuery();
        }

        public static bool IsEvent(this Type type)
        {
            return typeof(IEvent).IsAssignableFrom(type);
        }

        public static bool IsNServiceBusEvent(this Type type)
        {
            return type.IsEvent();
        }

    }
}