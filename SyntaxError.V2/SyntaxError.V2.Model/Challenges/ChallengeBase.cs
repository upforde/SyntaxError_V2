using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using SyntaxError.V2.Modell.Utility;

namespace SyntaxError.V2.Modell.Challenges
{
    /// <summary>The base class for all challenges</summary>
    [Table("Challenges")]
    public class ChallengeBase
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
