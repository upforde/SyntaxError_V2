using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using SyntaxError.V2.Modell.Challenges;

namespace SyntaxError.V2.Modell.ChallengeObjects
{
    /// <summary>The base class for all games, images and songs</summary>
    [Table("Objects")]
    public class MediaObject
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string URI { get; set; }
    }
}
