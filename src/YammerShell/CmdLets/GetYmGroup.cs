using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using YammerShell.YammerObjects;

namespace YammerShell.CmdLets
{
    [Cmdlet(VerbsCommon.Get, "YmGroup")]
    public class GetYmGroup : PSCmdlet
    {
        private Request _request;

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        Position = 0,
        HelpMessage = "ID of a yammer group"
        )]
        public int? Id { get; set; }

        protected override void ProcessRecord()
        {
            var token = SessionState.PSVariable.Get(Properties.Resources.TokenVariable);
            if (token == null)
            {
                WriteWarning(Properties.Resources.EmptyTokenWarning);
                return;
            }
            _request = new Request(token.Value.ToString());

            if (Id != null)
            {
                try
                {
                    var group = JToken.Parse(_request.Get(Properties.Resources.YammerApi + "groups/" + Id + ".json"));
                    WriteObject(GetYammerGroupFromJToken(group));
                }
                catch (Exception e)
                {
                    var errorRecord = new ErrorRecord(e, "id", ErrorCategory.InvalidArgument, Id);
                    WriteError(errorRecord);
                }
                return;
            }
            try
            {
                var groups = JArray.Parse(_request.Get(Properties.Resources.YammerApi + "groups.json"));
                var allYammerGroups = new List<YammerGroup>();
                foreach (var group in groups)
                {
                    var yammerGroup = GetYammerGroupFromJToken(group);
                    allYammerGroups.Add(yammerGroup);
                }
                WriteObject(allYammerGroups, true);
            }
            catch (Exception e)
            {
                var errorRecord = new ErrorRecord(e, "parse", ErrorCategory.InvalidArgument, "");
                WriteError(errorRecord);
            }
        }

        private YammerGroup GetYammerGroupFromJToken(JToken group)
        {
            var yammerGroup = new YammerGroup();
            yammerGroup.Id = Convert.ToInt64(group["id"]);
            yammerGroup.Email = (group["email"] ?? string.Empty).ToString();
            yammerGroup.FullName = group["full_name"].ToString();
            yammerGroup.Name = group["name"].ToString();
            yammerGroup.NetworkId = Convert.ToInt64(group["network_id"]);
            yammerGroup.Description = (group["description"] ?? string.Empty).ToString();
            yammerGroup.Privacy = group["privacy"].ToString();
            yammerGroup.Url = (group["web_url"] ?? string.Empty).ToString();
            yammerGroup.CreatedAt = (DateTime)(group["created_at"] ?? DateTime.MinValue);
            yammerGroup.CreatorId = Convert.ToInt64(group["creator_id"] ?? -1);
            yammerGroup.Members = Convert.ToInt32(group["stats"]["members"]); 
            return yammerGroup;
        }
    }
}
