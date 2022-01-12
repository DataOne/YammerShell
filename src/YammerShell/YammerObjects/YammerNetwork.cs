using System;

namespace YammerShell.YammerObjects
{
    public class YammerNetwork
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool Paid { get; set; }
        public bool IsOrgChartEnabled { get; set; }
        public bool IsGroupEnabled { get; set; }
        public bool IsChatEnabled { get; set; }
        public bool IsTranslationEnabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool AllowSamlAuthentication { get; set; }
        public bool EnforceOfficeAuthentication { get; set; }
    }
}