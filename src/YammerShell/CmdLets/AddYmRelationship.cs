using System;
using System.Management.Automation;
using System.Text;

namespace YammerShell.CmdLets
{
    [Cmdlet(VerbsCommon.Add, "YmRelationship")]
    public class AddYmRelationship : PSCmdlet
    {
        private Request _request;

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        Position = 0,
        HelpMessage = "ID of a user"
        )]
        public int? UserId { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "Emails of users that are subordinates"
        )]
        public string[] Subordinates { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "Emails of users that are superior"
        )]
        public string[] Superiors { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "Emails of users that are colleagues"
        )]
        public string[] Colleagues { get; set; }

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
                if (Subordinates == null && Superiors == null && Colleagues == null)
                {
                    var errorRecord = new ErrorRecord(new ArgumentNullException(), "Missing parameters", ErrorCategory.InvalidArgument, Subordinates);
                    WriteError(errorRecord);
                    return;
                }

                var url = string.Format("{0}relationships.json", Properties.Resources.YammerApi);
                var postData = new StringBuilder();

                if (UserId.HasValue)
                {
                    postData.Append("user_id=" + UserId);
                    postData.Append("&");
                }
                if (Subordinates != null && Subordinates.Length > 0)
                {
                    postData.Append("subordinate=" + Subordinates[0]);
                    for (int i = 1; i < Subordinates.Length; i++)
                    {
                        postData.Append("&subordinate=" + Subordinates[i]);
                    }
                    if (Superiors.Length > 0 || Colleagues.Length > 0)
                    {
                        postData.Append("&");
                    }
                }
                if (Superiors != null && Superiors.Length > 0)
                {
                    postData.Append("superior=" + Superiors[0]);
                    for (int i = 1; i < Superiors.Length; i++)
                    {
                        postData.Append("&superior=" + Superiors[i]);
                    }
                    if (Colleagues.Length > 0)
                    {
                        postData.Append("&");
                    }
                }
                if (Colleagues != null && Colleagues.Length > 0)
                {
                    postData.Append("colleagues=" + Colleagues[0]);
                    for (int i = 1; i < Colleagues.Length; i++)
                    {
                        postData.Append("&colleagues=" + Colleagues[i]);
                    }
                }

                _request.Post(url, postData.ToString()); // TODO test as admin
            }
            catch (Exception e)
            {
                var errorRecord = new ErrorRecord(e, "94", ErrorCategory.InvalidArgument, UserId);
                WriteError(errorRecord);
            }
        }

    }
}
