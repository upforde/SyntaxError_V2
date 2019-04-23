using SyntaxError.V2.Modell.Utility;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyntaxError.V2.Modell.Challenges
{
    [Table("Challenges")]
    public abstract class ChallengeBase
    {
        [Key]
        public int ChallengeID { get; set; }
        [Required]
        public string ChallengeTask { get; set; }

        public ICollection<UsingChallenge> UsedIn { get; set; } = new List<UsingChallenge>();

        public string GetDiscriminator()
        {
            return GetType().Name;
        }
    }
}
