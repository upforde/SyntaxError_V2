using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxError.V2.App.Helpers
{
    public class GroupChallengesList : List<object>
{
    public GroupChallengesList(IEnumerable<object> items) : base(items)
    {
    }
    public object Key { get; set; }
}

}
