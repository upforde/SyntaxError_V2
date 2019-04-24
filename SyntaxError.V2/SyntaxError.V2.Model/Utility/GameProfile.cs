using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyntaxError.V2.Modell.Utility
{
    public class GameProfile
    {
        [Key]
        public int ID { get; set; }
        public DateTime DateCreated { get; set; }

        public int? SaveGameID { get; set; }
        public SaveGame SaveGame { get; set; }
        
        public int? ProfileID { get; set; }
        public Profile Profile { get; set; }
    }
}
