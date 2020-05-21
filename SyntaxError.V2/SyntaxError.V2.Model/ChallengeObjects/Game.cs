using System.Collections.Generic;

using SyntaxError.V2.Modell.Challenges;

namespace SyntaxError.V2.Modell.ChallengeObjects
{
    /// <summary><see cref="MediaObject"/> of type <see cref="Game"/></summary>
    public class Game: MediaObject
    {
        public ICollection<GameChallenge> Challenges { get; set; }
    }
}
