using System;
using System.Collections.Generic;

namespace YammerShell.YammerObjects
{
    public class YammerUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public string Timezone { get; set; }
        public int NetworkId { get; set; }
        public string NetworkName { get; set; }
        public IEnumerable<string> PhoneNumbers { get; set; }
        public DateTime ActivatedAt { get; set; }
        public string Url { get; set; }
    }
}
