using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using SyntaxError.V2.Modell.Challenges;

namespace SyntaxError.V2.Modell.ChallengeObjects
{
    /// <summary>DEPRECATED. Was meant for assigning crew members to CrewChallenge</summary>
    [Table("CrewMembers")]
    public class CrewMember
    {
        [Key]
        public int CrewMemberID { get; set; }
        [Required]
        public string CrewTag { get; set; }
    }
}
