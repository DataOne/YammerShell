using System.Collections.Generic;

namespace YammerShell.YammerObjects
{
    public class YammerSearchResult
    {
        public int Page { get; set; }
        public int TotalMessages { get; set; }
        public int TotalGroups { get; set; }
        public int TotalTopics { get; set; }
        public int TotalFiles { get; set; }
        public int TotalUsers { get; set; }

        public IEnumerable<YammerMessage> Messages { get; set; }
        public IEnumerable<YammerGroup> Groups { get; set; }
        public IEnumerable<YammerTopic> Topics { get; set; }
        public IEnumerable<YammerFile> Files { get; set; }
        public IEnumerable<YammerUser> Users { get; set; }
    }
}
