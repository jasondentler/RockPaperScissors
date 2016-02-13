using System;
using System.Linq;
using RockPaperScissors.Domain.GamePlay;

namespace RockPaperScissors.Domain
{
    public static class GameMother
    {
        private static readonly Random Random = new Random();

        public static Game GenerateStartedGame(int numberOfPlayers = 2)
        {
            var game = new Game()
            {
                Id = Random.Next()
            };

            var players = PlayerMother.GeneratePlayers(numberOfPlayers);
            foreach (var player in players)
                game.Join(player);
            game.Start();

            DomainEvents.GetEvents().ToArray();

            return game;
        }
    }
}