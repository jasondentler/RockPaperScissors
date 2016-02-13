using NUnit.Framework;
using SharpTestsEx;

namespace RockPaperScissors.Messages
{
    [TestFixture]
    public class MessageConventionsIsMessageShould
    {

        [Test]
        public void ReturnTrueForCommands()
        {
            typeof(TestMessageTypes.Command).IsMessage().Should().Be.True();
        }

        [Test]
        public void ReturnFalseForQueries()
        {
            typeof(TestMessageTypes.Query).IsMessage().Should().Be.True();
        }

        [Test]
        public void ReturnTrueForEvents()
        {
            typeof(TestMessageTypes.Event).IsMessage().Should().Be.True();
        }

        [Test]
        public void ReturnTrueForMessages()
        {
            typeof(TestMessageTypes.Message).IsMessage().Should().Be.True();
        }

        [Test]
        public void ReturnFalseForNotAMessage()
        {
            typeof(TestMessageTypes.NotAMessage).IsMessage().Should().Be.False();
        }

    }
}
