using System;
using System.Collections.Generic;

namespace YammerShell.YammerObjects
{
    public class YammerMessage
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string RepliedToId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string NetworkId { get; set; }
        public string MessageType { get; set; }
        public string GroupId { get; set; }
        public string Body { get; set; }
        public IEnumerable<string> AttachmentUrls { get; set; }
        public string Url { get; set; }
    }
}
