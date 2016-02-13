using NUnit.Framework;
using SharpTestsEx;

namespace RockPaperScissors.Messages
{
    [TestFixture]
    public class MessageConventionsIsCommandShould
    {

        [Test]
        public void ReturnTrueForCommands()
        {
            typeof(TestMessageTypes.Command).IsCommand().Should().Be.True();
        }

        [Test]
        public void ReturnFalseForQueries()
        {
            typeof (TestMessageTypes.Query).IsCommand().Should().Be.False();
        }

        [Test]
        public void ReturnFalseForEvents()
        {
            typeof(TestMessageTypes.Event).IsCommand().Should().Be.False();
        }

        [Test]
        public void ReturnFalseForMessages()
        {
            typeof(TestMessageTypes.Message).IsCommand().Should().Be.False();
        }

        [Test]
        public void ReturnFalseForNotAMessage()
        {
            typeof(TestMessageTypes.NotAMessage).IsCommand().Should().Be.False();
        }

    }
}
