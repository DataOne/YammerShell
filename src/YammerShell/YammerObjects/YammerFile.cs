using System;

namespace YammerShell.YammerObjects
{
    public class YammerFile
    {
        public int Id { get; set; }
        public int NetworkId { get; set; }
        public int? GroupId { get; set; }
        public int? OwnerId { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContentClass { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Privacy { get; set; }
    }
}