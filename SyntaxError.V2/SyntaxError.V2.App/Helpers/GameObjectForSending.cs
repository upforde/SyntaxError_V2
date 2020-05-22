using SyntaxError.V2.Modell.Utility;
using System.Collections.Generic;

namespace SyntaxError.V2.App.Helpers
{
    /// <summary>Class that houses the selected GameProfile and the relevant challenges. This object gets sent to both the Game page and the Admin page.</summary>
    public class GameObjectForSending
    {
        /// <summary>Gets or sets the game profile.</summary>
        /// <value>The game profile.</value>
        public GameProfile GameProfile { get; set; }

        /// <summary>Gets or sets the challenges.</summary>
        /// <value>The challenges.</value>
        public List<ListItemMainPage> Challenges { get; set; }
    }
}
