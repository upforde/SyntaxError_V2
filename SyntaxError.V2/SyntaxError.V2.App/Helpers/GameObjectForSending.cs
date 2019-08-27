using SyntaxError.V2.Modell.Challenges;
using SyntaxError.V2.Modell.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxError.V2.App.Helpers
{
    public class GameObjectForSending
    {
        public GameProfile GameProfile { get; set; }

        public List<ListItemMainPage> Challenges { get; set; }
    }
}
