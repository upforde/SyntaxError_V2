using System.ComponentModel.DataAnnotations.Schema;

using SyntaxError.V2.Modell.Challenges;

namespace SyntaxError.V2.Modell.Utility
{
    /// <summary>Class that binds a <see cref="ChallengeBase"/> to a <see cref="UsingBase"/></summary>
    [Table("UsingProfileToChallenge")]
    public class UsingChallenge
    {
        public int? ChallengeID { get; set; }
        public ChallengeBase Challenge { get; set; }
        public int? UsingID { get; set; }
        public UsingBase UsingPane { get; set; }
    }
}
