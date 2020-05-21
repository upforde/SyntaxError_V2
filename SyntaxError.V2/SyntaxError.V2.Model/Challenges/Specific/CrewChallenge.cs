using SyntaxError.V2.Modell.ChallengeObjects;

namespace SyntaxError.V2.Modell.Challenges
{
    /// <summary>Challenge where a contestant has to compete against a crew member in a game</summary>
    public class CrewChallenge: GameChallenge
    {
        public int? CrewMemberID { get; set; }
        public CrewMember CrewMember { get; set; }
    }
}
