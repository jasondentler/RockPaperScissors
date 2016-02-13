using System.Linq;
using NUnit.Framework;
using RockPaperScissors.Domain.GamePlay;
using RockPaperScissors.Messages;
using SharpTestsEx;

namespace RockPaperScissors.Domain
{
    public class GameStartShould : TestBase
    {

        [SetUp]
        public void Setup()
        {
            DomainEvents.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            DomainEvents.Clear();
        }

        [Test]
        public void ThrowWhenNoPlayersSupplied()
        {
            var game = new Game();
            Assert.Throws<InvalidPlayerCountException>(() => game.Start());
        }

        [Test]
        public void ThrowWhenOnlyOnePlayerSupplied()
        {
            var game = new Game();
            var player = PlayerMother.GeneratePlayer();
            game.Join(player);
            Assert.Throws<InvalidPlayerCountException>(() => game.Start());
        }

        [Test]
        public void PublishGameStarted()
        {
            var game = new Game();
            var players = PlayerMother.GeneratePlayers(2);
            foreach (var player in players)
                game.Join(player);
            
            game.Start();
            var e = DomainEvents.GetEvents().OfType<GameStarted>().Single();
            e.GameId.Should().Be.EqualTo(game.Id);
        }
    }
}