using System;
using System.Management.Automation;
using System.Text;

namespace YammerShell.CmdLets
{
    [Cmdlet(VerbsCommunications.Send, "YmInvitation")]
    public class SendYmInvitation : PSCmdlet
    {
        private Request _request;

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        Mandatory = true,
        Position = 0,
        HelpMessage = "Emails of users which have not yet joined the network."
        )]
        public string[] Emails { get; set; }

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
                var url = new StringBuilder();
                url.Append(Properties.Resources.YammerApi + "invitations.json?email=" + Emails[0]);

                for (int i = 1; i < Emails.Length; i++)
                {
                    url.Append("&email=");
                    url.Append(Emails[i]);
                }
                _request.Post(url.ToString(), string.Empty); // TODO test as admin
            }
            catch (Exception e)
            {
                var errorRecord = new ErrorRecord(e, "01", ErrorCategory.InvalidArgument, Emails);
                WriteError(errorRecord);
            }
        }
    }
}
