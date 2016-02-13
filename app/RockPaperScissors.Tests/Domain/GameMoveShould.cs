using System;
using System.Linq;
using NUnit.Framework;
using RockPaperScissors.Domain.GamePlay;
using RockPaperScissors.Messages;
using SharpTestsEx;
using Moves = RockPaperScissors.Domain.GamePlay.Moves;

namespace RockPaperScissors.Domain
{
    public class GameMoveShould : TestBase
    {



        [Test]
        public void ThrowWhenGameNotStarted()
        {
            var game = new Game();
            var players = PlayerMother.GeneratePlayers(2).ToArray();

            foreach (var player in players)
                game.Join(player);

            Assert.Throws<InvalidGameStateException>(() => game.Move(players.First(), Moves.Paper));
        }

        [Test]
        public void ThrowsWhenPlayerIsNull()
        {
            var game = GameMother.GenerateStartedGame(2);
            Assert.Throws<ArgumentNullException>(() => game.Move(null, Moves.Paper));
        }

        [Test]
        public void ThrowsWhenPlayerAlreadyMoved()
        {
            var game = GameMother.GenerateStartedGame(2);
            var playerOne = game.Players.First();
            game.Move(playerOne, Moves.Paper);
            Assert.Throws<PlayerAlreadyMovedException>(() => game.Move(playerOne, Moves.Rock));
        }

        [Test]
        public void ThrowsWhenPlayerNotInTheGame()
        {
            var game = GameMother.GenerateStartedGame(2);
            var nonPlayer = PlayerMother.GeneratePlayer();
            Assert.Throws<PlayerNotInTheGameException>(() => game.Move(nonPlayer, Moves.Rock));
        }

        [Test]
        public void RecordThePlayersMove()
        {
            var game = GameMother.GenerateStartedGame();
            var playerOne = game.Players.First();
            game.Move(playerOne, Moves.Rock);
            game.Moves[playerOne].Should().Be.EqualTo(Moves.Rock);
        }

        [Test]
        public void PublishPlayerMoved()
        {
            var game = GameMother.GenerateStartedGame();
            var player = game.Players.First();
            const Moves move = Moves.Paper;

            game.Move(player, move);
            
            var e = (PlayerMoved) DomainEvents.GetEvents().Single();

            e.GameId.Should().Be.EqualTo(game.Id);
            e.PlayerId.Should().Be.EqualTo(player.PlayerId);
            e.Move.Should().Be.EqualTo((RockPaperScissors.Messages.Moves) move);
        }

        [Test]
        public void ChangeStateToCompletedWhenGameCompletes()
        {
            var game = GameMother.GenerateStartedGame();
            foreach (var player in game.Players)
                game.Move(player, Moves.Paper);

            game.State.Should().Be.EqualTo(GameStates.Completed);
        }

        [Test]
        public void PublishGameCompletedWhenGameCompletes()
        {
            var game = GameMother.GenerateStartedGame();
            foreach (var player in game.Players)
                game.Move(player, Moves.Paper);

            var events = DomainEvents.GetEvents().ToArray();
            var gameCompleted = events.OfType<GameCompleted>().Single();
            gameCompleted.GameId.Should().Be.EqualTo(game.Id);
        }

        [Test]
        public void PublishGameTiedWhenGameTies()
        {
            var game = GameMother.GenerateStartedGame();
            foreach (var player in game.Players)
                game.Move(player, Moves.Paper);

            var events = DomainEvents.GetEvents().ToArray();
            var gameTied = events.OfType<GameTied>().Single();
            gameTied.GameId.Should().Be.EqualTo(game.Id);
        }

        [Test]
        public void PublishGameWonWhenGameIsWon()
        {
            const int numberOfPlayers = 10;
            const int numberOfWinners = 5;
            var game = GameMother.GenerateStartedGame(numberOfPlayers);
            var players = game.Players.ToArray();
            var winners = players.Take(numberOfWinners).ToArray();
            var losers = players.Except(winners).ToArray();

            foreach (var player in winners)
                game.Move(player, Moves.Paper);

            foreach (var player in losers)
                game.Move(player, Moves.Rock);

            var events = DomainEvents.GetEvents().ToArray();
            var gameWonEvents = events.OfType<GameWon>().ToArray();

            // Make sure we have the right number of winners
            gameWonEvents.Should().Have.Count.EqualTo(numberOfWinners);

            // Make sure the right players won
            gameWonEvents.Select(e => e.PlayerId).Should().Have.SameValuesAs(winners.Select(p => p.PlayerId));

            // Make sure they won the right game
            foreach (var gameWonEvent in gameWonEvents)
                gameWonEvent.GameId.Should().Be.EqualTo(game.Id);
        }

    }
}
