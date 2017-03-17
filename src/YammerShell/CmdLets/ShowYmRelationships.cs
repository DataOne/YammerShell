using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using YammerShell.YammerObjects;

namespace YammerShell.CmdLets
{
    [Cmdlet(VerbsCommon.Show, "YmRelationships")]
    public class ShowYmRelationships : PSCmdlet
    {
        private Request _request;

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        Position = 0,
        HelpMessage = "ID of a user",
        ParameterSetName = "Id"
        )]
        public int? UserId { get; set; }

        protected override void ProcessRecord()
        {
            var token = SessionState.PSVariable.Get(Properties.Resources.TokenVariable);
            if (token == null)
            {
                WriteWarning(Properties.Resources.EmptyTokenWarning);
                return;
            }
            _request = new Request(token.Value.ToString());

            try
            {
                var response = _request.Get(Properties.Resources.YammerApi + "relationships.json?user_id=" + UserId);

                var relationships = JObject.Parse(response);
                var allSubordinates = JArray.Parse(relationships["subordinates"].ToString());
                var allSuperiors = JArray.Parse(relationships["superiors"].ToString());
                var allColleagues = JArray.Parse(relationships["colleagues"].ToString());

                var yammerRelationship = new YammerRelationship();
                var subordinates = new List<YammerUser>();
                var colleagues = new List<YammerUser>();
                var superiors = new List<YammerUser>();

                foreach (var subordinate in allSubordinates)
                {
                    subordinates.Add(GetYammerUser(subordinate));
                }
                yammerRelationship.Subordinates = subordinates;

                foreach (var colleague in allColleagues)
                {
                    colleagues.Add(GetYammerUser(colleague));
                }
                yammerRelationship.Colleagues = colleagues;

                foreach (var superior in allSuperiors)
                {
                    superiors.Add(GetYammerUser(superior));
                }
                yammerRelationship.Superiors = superiors;

                WriteObject(yammerRelationship);
            }
            catch (Exception e)
            {
                var errorRecord = new ErrorRecord(e, "94", ErrorCategory.InvalidArgument, UserId);
                WriteError(errorRecord);
            }
        }

        private YammerUser GetYammerUser(JToken user)
        {
            var yammerUser = new YammerUser();
            yammerUser.UserName = GetToken(user, "name");
            yammerUser.Id = Convert.ToInt32(GetToken(user, "id"));
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
            foreach (var number in new JArray(user["phone_numbers"]).Children())
            {
                phoneNumbers.Add(number.ToString());
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
