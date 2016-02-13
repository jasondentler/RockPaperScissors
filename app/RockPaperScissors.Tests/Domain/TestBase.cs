using System.Linq;
using NUnit.Framework;

namespace RockPaperScissors.Domain
{

    [TestFixture]
    public abstract class TestBase
    {

        [SetUp]
        public void Setup()
        {
            DomainEvents.GetEvents().ToArray();
        }

        [TearDown]
        public void TearDown()
        {
            DomainEvents.GetEvents().ToArray();
        }

    }
}
