using SyntaxError.V2.Modell.ChallengeObjects;

namespace SyntaxError.V2.Modell.Challenges
{
    public class CrewChallenge: GameChallenge
    {
        public int? CrewMemberID { get; set; }
        public CrewMember CrewMember { get; set; }
    }
}
