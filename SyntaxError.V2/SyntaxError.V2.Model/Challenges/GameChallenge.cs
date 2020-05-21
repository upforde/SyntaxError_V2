using SyntaxError.V2.Modell.ChallengeObjects;

namespace SyntaxError.V2.Modell.Challenges
{
    /// <summary>Challenge that has something to do with a <see cref="Modell.ChallengeObjects.Game"/></summary>
    public abstract class GameChallenge: ChallengeBase
    {
        public int? GameID { get; set; }
        public Game Game { get; set; }
    }
}
