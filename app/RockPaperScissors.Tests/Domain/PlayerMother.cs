using System;
using System.Collections.Generic;
using System.Linq;

namespace RockPaperScissors.Domain
{
    public static class PlayerMother
    {

        private static readonly Random Random = new Random();

        public static Player GeneratePlayer()
        {
            var id = Random.Next();
            return new Player()
            {
                Id = id,
                Name = "Player " + id
            };
        }

        private static IEnumerable<Player> GeneratePlayers()
        {
            while (true)
                yield return GeneratePlayer();
        }

        public static IEnumerable<Player> GeneratePlayers(int numberOfPlayers)
        {
            var players = GeneratePlayers().Take(numberOfPlayers).ToArray();
            DomainEvents.GetEvents().ToArray();
            return players;
        }
    }
}