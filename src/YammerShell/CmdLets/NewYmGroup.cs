using Newtonsoft.Json.Linq;
using System;
using System.Management.Automation;

namespace YammerShell.CmdLets
{
    [Cmdlet(VerbsCommon.New, "YmGroup")]
    public class NewYmGroup : PSCmdlet
    {
        private Request _request;

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        Mandatory = true,
        Position = 0,
        HelpMessage = "Name of the new yammer group"
        )]
        public string Name { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "Description of the new yammer group"
        )]
        public string Description { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "If the group should be private"
        )]
        public SwitchParameter Private { get; set; }

        protected override void ProcessRecord()
        {
            var token = SessionState.PSVariable.Get(Properties.Resources.TokenVariable);
            if (token == null)
            {
                WriteWarning(Properties.Resources.EmptyTokenWarning);
                return;
            }
            _request = new Request(token.Value.ToString());

            string requestUrl = Properties.Resources.YammerApi + "groups.json?name=" + Name;
            if (Private.IsPresent)
            {
                requestUrl += "&private=true";
            }
            if (Description != null)
            {
                requestUrl += "&description=" + Description;
            }
            try
            {
                var response = _request.Post(requestUrl, string.Empty);
                var newGroup = JObject.Parse(response);
                WriteObject(Convert.ToInt64(newGroup["id"]));
            }
            catch (Exception e)
            {
                var errorRecord = new ErrorRecord(e, "94", ErrorCategory.InvalidArgument, Name);
                WriteError(errorRecord);
            }
        }
    }
}
