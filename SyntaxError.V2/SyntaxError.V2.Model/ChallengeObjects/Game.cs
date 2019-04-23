using SyntaxError.V2.Modell.Challenges;
using System.Collections.Generic;

namespace SyntaxError.V2.Modell.ChallengeObjects
{
    public class Game: OuterSourceObject
    {
        public ICollection<GameChallenge> Challenges { get; set; }
    }
}
