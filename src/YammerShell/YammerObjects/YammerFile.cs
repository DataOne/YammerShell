using System;

namespace YammerShell.YammerObjects
{
    public class YammerFile
    {
        public long Id { get; set; }
        public long NetworkId { get; set; }
        public long? GroupId { get; set; }
        public long? OwnerId { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContentClass { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Privacy { get; set; }
    }
}