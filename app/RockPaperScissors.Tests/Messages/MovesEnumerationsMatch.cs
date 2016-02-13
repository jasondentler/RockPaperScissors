using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpTestsEx;

namespace RockPaperScissors.Messages
{
    [TestFixture]
    public class MovesEnumerationShould
    {

        [Test]
        public void MatchDomainMovesEnumeration()
        {
            var messageMoves = GetEnumerationValues(typeof (RockPaperScissors.Messages.Moves));
            var domainMoves = GetEnumerationValues(typeof (Domain.GamePlay.Moves));
            messageMoves.Should().Have.SameValuesAs(domainMoves);
        }

        private static IDictionary<string, int> GetEnumerationValues(Type type)
        {
            return Enum.GetValues(type)
                .Cast<int>()
                .ToDictionary(value => Enum.GetName(type, value), value => value);
        }

    }
}
