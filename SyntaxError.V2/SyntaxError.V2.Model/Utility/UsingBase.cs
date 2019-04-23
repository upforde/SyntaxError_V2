using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyntaxError.V2.Modell.Utility
{
    [Table("UsingProfiles")]
    public abstract class UsingBase
    {
        [Key]
        public int UsingID { get; set; }
        public ICollection<UsingChallenge> Challenges { get; set; } = new List<UsingChallenge>();
    }
}
