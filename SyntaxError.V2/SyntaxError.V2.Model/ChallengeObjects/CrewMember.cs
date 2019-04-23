using SyntaxError.V2.Modell.Challenges;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyntaxError.V2.Modell.ChallengeObjects
{
    [Table("CrewMembers")]
    public class CrewMember
    {
        [Key]
        public int CrewMemberID { get; set; }
        [Required]
        public string CrewTag { get; set; }
    }
}
