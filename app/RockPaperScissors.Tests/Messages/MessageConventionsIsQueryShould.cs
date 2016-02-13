using NUnit.Framework;
using SharpTestsEx;

namespace RockPaperScissors.Messages
{
    [TestFixture]
    public class MessageConventionsIsQueryShould
    {

        [Test]
        public void ReturnFalseForCommands()
        {
            typeof(TestMessageTypes.Command).IsQuery().Should().Be.False();
        }
        
        [Test]
        public void ReturnTrueForQueries()
        {
            typeof(TestMessageTypes.Query).IsQuery().Should().Be.True();
        }

        [Test]
        public void ReturnTrueForEvents()
        {
            typeof(TestMessageTypes.Event).IsQuery().Should().Be.False();
        }

        [Test]
        public void ReturnFalseForMessages()
        {
            typeof(TestMessageTypes.Message).IsQuery().Should().Be.False();
        }

        [Test]
        public void ReturnFalseForNotAMessage()
        {
            typeof(TestMessageTypes.NotAMessage).IsQuery().Should().Be.False();
        }

    }
}
