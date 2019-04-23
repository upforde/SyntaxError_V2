using SyntaxError.V2.Modell.ChallengeObjects;

namespace SyntaxError.V2.Modell.Challenges
{
    public abstract class GameChallenge: ChallengeBase
    {
        public int? GameID { get; set; }
        public Game Game { get; set; }
    }
}
