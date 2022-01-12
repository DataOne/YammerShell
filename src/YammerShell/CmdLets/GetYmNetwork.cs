using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using YammerShell.YammerObjects;

namespace YammerShell.CmdLets
{
    [Cmdlet(VerbsCommon.Get, "YmNetwork")]
    public class GetYmNetwork : PSCmdlet
    {
        private Request _request;

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        Position = 0,
        HelpMessage = "With this parameter set you request all networks the user has access to but filtered by id."
        )]
        public long? Id { get; set; }

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
                var result = _request.Get(Properties.Resources.YammerApi + "networks/current.json?include_suspended=true");

                var yammerNetworks = new List<YammerNetwork>();
                foreach (var network in JArray.Parse(result))
                {
                    var networkId = Convert.ToInt64(network["id"]);
                    if (!Id.HasValue || Id == networkId)
                    {
                        yammerNetworks.Add(GetYammerNetwork(network, networkId));
                    }
                }
                WriteObject(yammerNetworks);
            }
            catch (Exception e)
            {
                var errorRecord = new ErrorRecord(e, "42", ErrorCategory.InvalidArgument, "");
                WriteError(errorRecord);
            }
        }

        private YammerNetwork GetYammerNetwork(JToken network, long id)
        {
            var yammerNetwork = new YammerNetwork();
            yammerNetwork.Id = id;
            yammerNetwork.Email = network["email"].ToString();
            yammerNetwork.Name = network["name"].ToString();
            yammerNetwork.Url = network["web_url"].ToString();
            yammerNetwork.Paid = (bool)network["paid"];
            yammerNetwork.IsOrgChartEnabled = (bool)network["is_org_chart_enabled"];
            yammerNetwork.IsGroupEnabled = (bool)network["is_group_enabled"];
            yammerNetwork.IsChatEnabled = (bool)network["is_chat_enabled"];
            yammerNetwork.IsTranslationEnabled = (bool)network["is_translation_enabled"];
            yammerNetwork.CreatedAt = (DateTime)network["created_at"];
            yammerNetwork.AllowSamlAuthentication = (bool)network["allow_saml_authentication"];
            yammerNetwork.EnforceOfficeAuthentication = (bool)network["enforce_office_authentication"];
            return yammerNetwork;
        }
    }
}
