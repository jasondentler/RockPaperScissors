using NUnit.Framework;
using SharpTestsEx;

namespace RockPaperScissors.Messages
{
    [TestFixture]
    public class MessageConventionsIsEventShould
    {

        [Test]
        public void ReturnFalseForCommands()
        {
            typeof(TestMessageTypes.Command).IsEvent().Should().Be.False();
        }
        
        [Test]
        public void ReturnFalseForQueries()
        {
            typeof(TestMessageTypes.Query).IsEvent().Should().Be.False();
        }

        [Test]
        public void ReturnTrueForEvents()
        {
            typeof(TestMessageTypes.Event).IsEvent().Should().Be.True();
        }

        [Test]
        public void ReturnFalseForMessages()
        {
            typeof(TestMessageTypes.Message).IsEvent().Should().Be.False();
        }

        [Test]
        public void ReturnFalseForNotAMessage()
        {
            typeof(TestMessageTypes.NotAMessage).IsEvent().Should().Be.False();
        }

    }
}
