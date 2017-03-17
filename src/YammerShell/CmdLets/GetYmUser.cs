using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using YammerShell.YammerObjects;

namespace YammerShell.CmdLets
{
    [Cmdlet(VerbsCommon.Get, "YmUser", DefaultParameterSetName = "Id")]
    public class GetYmUser : PSCmdlet
    {
        private Request _request;

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        Position = 0,
        HelpMessage = "ID of a user",
        ParameterSetName = "Id"
        )]
        public int? Id { get; set; }

        [ValidateRange(1,1000)]
        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "Limit returned users amount",
        ParameterSetName = "Group"
        )]
        [Parameter(ParameterSetName = "Email")]
        [Parameter(ParameterSetName = "StartLetter")]
        [Parameter(ParameterSetName = "Id")]
        public int? Limit { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "Return users in reverse order"
        )]
        public SwitchParameter Reverse { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "Return the current user",
        ParameterSetName = "Current"
        )]
        public SwitchParameter Current { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "ID of a group",
        Mandatory = true,
        ParameterSetName = "Group"
        )]
        public int? GroupId { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        Mandatory = true,
        HelpMessage = "Email of a user",
        ParameterSetName = "Email"
        )]
        public string Email { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "Return users with usernames beginning with the given character",
        Mandatory = true,
        ParameterSetName = "StartLetter"
        )]
        public char? StartLetter { get; set; }

        protected override void ProcessRecord()
        {
            var token = SessionState.PSVariable.Get(Properties.Resources.TokenVariable);
            if (token == null)
            {
                WriteWarning(Properties.Resources.EmptyTokenWarning);
                return;
            }
            _request = new Request(token.Value.ToString());

            var yammerUsers = new List<YammerUser>();

            if (Current.IsPresent)
            {
                try
                {
                    var user = GetCurrentYammerUser();
                    WriteObject(user);
                }
                catch (Exception e)
                {
                    var errorRecord = new ErrorRecord(e, "3", ErrorCategory.InvalidArgument, Current);
                    WriteError(errorRecord);
                }
                return;
            }
            if (Id.HasValue)
            {
                try
                {
                    var user = GetYammerUser(Id.Value);
                    WriteObject(user);
                }
                catch (Exception e)
                {
                    var errorRecord = new ErrorRecord(e, "94", ErrorCategory.InvalidArgument, Id);
                    WriteError(errorRecord);
                }
                return;
            }
            if (Email != null)
            {
                try
                {
                    var users = GetYammerUsers(Email);
                    WriteObject(users);
                }
                catch (Exception e)
                {
                    var errorRecord = new ErrorRecord(e, "513", ErrorCategory.InvalidArgument, Email);
                    WriteError(errorRecord);
                }
                return;
            }
            if (GroupId.HasValue)
            {
                try
                {
                    var users = GetYammerUsersInGroup(GroupId);
                    WriteObject(users);
                }
                catch (Exception e)
                {
                    var errorRecord = new ErrorRecord(e, "g1", ErrorCategory.InvalidArgument, GroupId);
                    WriteError(errorRecord);
                }
                return;
            }
            try
            {
                WriteObject(GetYammerUsers(), true);
            }
            catch (Exception e)
            {
                var errorRecord = new ErrorRecord(e, "4566", ErrorCategory.InvalidArgument, Id);
                WriteError(errorRecord);
            }
        }

        private object GetCurrentYammerUser()
        {
            var result = _request.Get(string.Format("{0}users/current.json", Properties.Resources.YammerApi));
            var user = JObject.Parse(result);
            return GetYammerUser(user);
        }

        private IEnumerable<YammerUser> GetYammerUsersInGroup(int? groupId)
        {
            var result = _request.Get(string.Format("{0}users/in_group/{1}.json?reverse={2}", Properties.Resources.YammerApi, GroupId, Reverse.IsPresent));
            var groupUsers = JObject.Parse(result);
            var users = JArray.Parse(groupUsers["users"].ToString());

            var allYammerUsers = new List<YammerUser>();
            foreach (var user in users)
            {
                allYammerUsers.Add(GetYammerUser(user));
            }
            return allYammerUsers;
        }

        public YammerUser GetYammerUser(int id)
        {
            var result = _request.Get(string.Format("{0}users/{1}.json", Properties.Resources.YammerApi, id));
            var user = JObject.Parse(result);
            return GetYammerUser(user);
        }

        public IEnumerable<YammerUser> GetYammerUsers(string email)
        {
            var result = _request.Get(string.Format("{0}users/by_email.json?email={1}&reverse={2}", Properties.Resources.YammerApi, email, Reverse.IsPresent));
            var users = JArray.Parse(result);

            var allYammerUsers = new List<YammerUser>();
            foreach (var user in users)
            {
                allYammerUsers.Add(GetYammerUser(user));
            }
            return allYammerUsers;
        }

        public IEnumerable<YammerUser> GetYammerUsers()
        {
            var letterParameter = string.Empty;
            if (StartLetter.HasValue)
            {
                letterParameter = "&letter=" + StartLetter;
            }

            var allYammerUsers = new List<YammerUser>();
            var requestsStarted = DateTime.Now.Ticks;
            for (int page = 1; ; page++)
            {
                var result = _request.Get(string.Format("{0}users.json?page={1}&reverse={2}{3}", Properties.Resources.YammerApi, page, Reverse.IsPresent, letterParameter));
                var users = JArray.Parse(result);
                
                foreach (var user in users)
                {
                    if (Limit == null || allYammerUsers.Count < Limit)
                    {
                        allYammerUsers.Add(GetYammerUser(user));
                    }
                    else
                    {
                        return allYammerUsers;
                    }
                }
                if (users.Count == 0)
                {
                    break;
                }
                // 50 users will be shown per page
                if (page * 50 >= Limit && (Limit - page * 50) <= 0)
                {
                    break;
                }
                // wait to prevent 10 requests in 10 seconds
                if (page % 10 == 0)
                {
                    if (requestsStarted + TimeSpan.FromSeconds(9).Ticks >= DateTime.Now.Ticks)
                    {
                        var PowerShellInstance = PowerShell.Create();
                        PowerShellInstance.AddCommand("Start-Sleep");
                        PowerShellInstance.AddParameter("Seconds", 2);
                        PowerShellInstance.Invoke();
                        PowerShellInstance.Dispose();
                    }
                    requestsStarted = DateTime.Now.Ticks;
                }
            }
            return allYammerUsers;
        }

        private YammerUser GetYammerUser(JToken user)
        {
            var yammerUser = new YammerUser();
            yammerUser.Id = Convert.ToInt32(GetToken(user, "id"));
            yammerUser.UserName = GetToken(user, "name");
            yammerUser.FirstName = GetToken(user, "first_name");
            yammerUser.LastName = GetToken(user, "last_name");
            yammerUser.FullName = GetToken(user, "full_name");
            yammerUser.Email = GetToken(user, "email");
            yammerUser.JobTitle = GetToken(user, "job_title");
            yammerUser.Department = GetToken(user, "department");
            yammerUser.Timezone = GetToken(user, "timezone");
            yammerUser.NetworkId = Convert.ToInt32(GetToken(user, "network_id"));
            yammerUser.NetworkName = GetToken(user, "network_name");
            yammerUser.Url = GetToken(user, "web_url");
            var activatedAt = user["activated_at"];
            yammerUser.ActivatedAt = activatedAt.Type == JTokenType.Null ? DateTime.MinValue : (DateTime)user["activated_at"];

            var phoneNumbers = new List<string>();
            var numbers = user["contact"]["phone_numbers"];
            foreach (var number in numbers)
            {
                phoneNumbers.Add(number["number"].ToString());
            }
            yammerUser.PhoneNumbers = phoneNumbers;

            return yammerUser;
        }

        private string GetToken(JToken token, string key)
        {
            var value = token[key];
            if (value == null)
            {
                return "";
            }
            return value.ToString();
        }

    }
}
