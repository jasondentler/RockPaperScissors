using NUnit.Framework;
using SharpTestsEx;

namespace RockPaperScissors.Messages
{
    [TestFixture]
    public class MessageConventionsIsNServiceBusMessageShould
    {

        [Test]
        public void ReturnTrueForCommands()
        {
            typeof(TestMessageTypes.Command).IsNServiceBusMessage().Should().Be.True();
        }

        [Test]
        public void ReturnFalseForQueries()
        {
            typeof(TestMessageTypes.Query).IsNServiceBusMessage().Should().Be.True();
        }

        [Test]
        public void ReturnTrueForEvents()
        {
            typeof(TestMessageTypes.Event).IsNServiceBusMessage().Should().Be.True();
        }

        [Test]
        public void ReturnTrueForMessages()
        {
            typeof(TestMessageTypes.Message).IsNServiceBusMessage().Should().Be.True();
        }

        [Test]
        public void ReturnFalseForNotAMessage()
        {
            typeof(TestMessageTypes.NotAMessage).IsNServiceBusMessage().Should().Be.False();
        }

    }
}
