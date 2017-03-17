using System;

namespace YammerShell.YammerObjects
{
    public class YammerGroup
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public int NetworkId { get; set; }
        public string Description { get; set; }
        public string Privacy { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatorId { get; set; }
        public int Members { get; set; }
    }
}
