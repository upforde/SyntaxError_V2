using System.Collections.Generic;

using SyntaxError.V2.Modell.Challenges;

namespace SyntaxError.V2.Modell.ChallengeObjects
{
    public class Game: MediaObject
    {
        public ICollection<GameChallenge> Challenges { get; set; }
    }
}
