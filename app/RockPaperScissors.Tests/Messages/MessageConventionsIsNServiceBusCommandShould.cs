using NUnit.Framework;
using SharpTestsEx;

namespace RockPaperScissors.Messages
{
    [TestFixture]
    public class MessageConventionsIsNServiceBusCommandShould
    {

        [Test]
        public void ReturnTrueForCommands()
        {
            typeof(TestMessageTypes.Command).IsNServiceBusCommand().Should().Be.True();
        }

        [Test]
        public void ReturnTrueForQueries()
        {
            typeof (TestMessageTypes.Query).IsNServiceBusCommand().Should().Be.True();
        }

        [Test]
        public void ReturnFalseForEvents()
        {
            typeof(TestMessageTypes.Event).IsNServiceBusCommand().Should().Be.False();
        }

        [Test]
        public void ReturnFalseForMessages()
        {
            typeof(TestMessageTypes.Message).IsNServiceBusCommand().Should().Be.False();
        }

        [Test]
        public void ReturnFalseForNotAMessage()
        {
            typeof(TestMessageTypes.NotAMessage).IsNServiceBusCommand().Should().Be.False();
        }

    }
}
