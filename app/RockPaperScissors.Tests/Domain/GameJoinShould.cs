using System;
using System.Linq;
using NUnit.Framework;
using RockPaperScissors.Domain.GamePlay;
using RockPaperScissors.Messages;
using SharpTestsEx;
using Moves = RockPaperScissors.Domain.GamePlay.Moves;

namespace RockPaperScissors.Domain
{
    public class GameJoinShould : TestBase
    {
        [Test]
        public void ThrowOnNullPlayer()
        {
            var game = new Game();
            Assert.Throws<ArgumentNullException>(() => game.Join(null));
        }

        [Test]
        public void ThrowWhenGameStarted()
        {
            var game = new Game();
            game.Join(PlayerMother.GeneratePlayer());
            game.Join(PlayerMother.GeneratePlayer());
            game.Start();
            Assert.Throws<InvalidGameStateException>(() => game.Join(PlayerMother.GeneratePlayer()));
        }

        [Test]
        public void ThrowWhenGameCompleted()
        {
            var game = new Game();
            var players = PlayerMother.GeneratePlayers(2);

            foreach (var player in players)
                game.Join(player);

            game.Start();

            foreach (var player in players)
                game.Move(player, Moves.Paper);
            
            Assert.Throws<InvalidGameStateException>(() => game.Join(PlayerMother.GeneratePlayer()));
        }

        [Test]
        public void AddPlayerToCollection()
        {
            //Arrange
            var game = new Game();
            PlayerRef player = PlayerMother.GeneratePlayer();
            
            //Act
            game.Join(player);

            //Assert
            game.Players.Should().Contain(player);
        }

        [Test]
        public void PublishPlayerJoined()
        {
            //Arrange
            var game = new Game();
            PlayerRef player = PlayerMother.GeneratePlayer();

            //Act
            game.Join(player);

            //Assert
            var e = DomainEvents.GetEvents().Single();
            e.Should().Be.OfType<PlayerJoined>();
            ((PlayerJoined) e).PlayerId.Should().Be.EqualTo(player.PlayerId);
        }
    }
}