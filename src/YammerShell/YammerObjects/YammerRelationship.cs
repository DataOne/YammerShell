using System.Collections.Generic;

namespace YammerShell.YammerObjects
{
    public class YammerRelationship
    {
        public IEnumerable<YammerUser> Superiors { get; set; }
        public IEnumerable<YammerUser> Colleagues { get; set; }
        public IEnumerable<YammerUser> Subordinates { get; set; }
    }
}
