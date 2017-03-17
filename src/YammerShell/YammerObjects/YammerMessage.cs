using System;
using System.Collections.Generic;

namespace YammerShell.YammerObjects
{
    public class YammerMessage
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int? RepliedToId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int NetworkId { get; set; }
        public string MessageType { get; set; }
        public int? GroupId { get; set; }
        public string Body { get; set; }
        public IEnumerable<string> AttachmentUrls { get; set; }
        public string Url { get; set; }
    }
}
