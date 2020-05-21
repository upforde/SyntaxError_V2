using System;
using System.ComponentModel.DataAnnotations;

namespace SyntaxError.V2.Modell.Utility
{
    /// <summary>Game profile containing the <see cref="Utility.Profile"/> and <see cref="Utility.SaveGame"/></summary>
    public class GameProfile
    {
        [Key]
        public int ID { get; set; }
        public string GameProfileName { get; set; }
        public DateTime DateCreated { get; set; }

        public int? SaveGameID { get; set; }
        public SaveGame SaveGame { get; set; }
        
        public int? ProfileID { get; set; }
        public Profile Profile { get; set; }
    }
}
