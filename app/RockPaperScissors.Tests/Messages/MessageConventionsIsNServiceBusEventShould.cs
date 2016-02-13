using NUnit.Framework;
using SharpTestsEx;

namespace RockPaperScissors.Messages
{
    [TestFixture]
    public class MessageConventionsIsNServiceBusEventShould
    {

        [Test]
        public void ReturnFalseForCommands()
        {
            typeof(TestMessageTypes.Command).IsNServiceBusEvent().Should().Be.False();
        }
        
        [Test]
        public void ReturnFalseForQueries()
        {
            typeof(TestMessageTypes.Query).IsNServiceBusEvent().Should().Be.False();
        }

        [Test]
        public void ReturnTrueForEvents()
        {
            typeof(TestMessageTypes.Event).IsNServiceBusEvent().Should().Be.True();
        }

        [Test]
        public void ReturnFalseForMessages()
        {
            typeof(TestMessageTypes.Message).IsNServiceBusEvent().Should().Be.False();
        }

        [Test]
        public void ReturnFalseForNotAMessage()
        {
            typeof(TestMessageTypes.NotAMessage).IsNServiceBusEvent().Should().Be.False();
        }

    }
}
