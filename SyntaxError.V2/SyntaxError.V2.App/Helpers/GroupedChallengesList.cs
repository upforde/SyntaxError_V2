using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxError.V2.App.Helpers
{
    /// <summary>Helper class needed to group challenges of all types into categories, for the ListView with floating headers for each category. The categories are the types of challenge.</summary>
    public class GroupChallengesList : List<object>
    {
        /// <summary>Initializes a new instance of the <see cref="GroupChallengesList" /> class.</summary>
        /// <param name="items">The items.</param>
        public GroupChallengesList(IEnumerable<object> items) : base(items)
        {
        }
        /// <summary>Gets or sets the key.</summary>
        /// <value>The key.</value>
        public object Key { get; set; }
    }
}
