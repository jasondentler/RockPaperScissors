using NUnit.Framework;
using RockPaperScissors.Domain.GamePlay;
using SharpTestsEx;

namespace RockPaperScissors.Domain
{
    public class GameConstructorShould : TestBase
    {

        [Test]
        public void InitializeTheMovesCollection()
        {
            var game = new Game();
            game.Moves.Should().Not.Be.Null();
        }

        [Test]
        public void InitializeThePlayersCollection()
        {
            var game = new Game();
            game.Players.Should().Not.Be.Null();
        }

    }
}
