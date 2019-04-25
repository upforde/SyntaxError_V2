using SyntaxError.V2.Modell.Challenges;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyntaxError.V2.Modell.Utility
{
    [Table("UsingProfileToChallenge")]
    public class UsingChallenge
    {
        public int? ChallengeID { get; set; }
        public ChallengeBase Challenge { get; set; }
        public int? UsingID { get; set; }
        public UsingBase UsingPane { get; set; }
    }
}
