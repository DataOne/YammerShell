using System;
using System.Management.Automation;

namespace YammerShell.CmdLets
{
    [Cmdlet(VerbsCommon.Add, "YmGroupMembership", DefaultParameterSetName = "UserId")]
    public class AddYmGroupMembership : PSCmdlet
    {
        private Request _request;

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        Mandatory = true,
        Position = 0,
        HelpMessage = "ID of a yammer group"
        )]
        public int? GroupId { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "ID of a user to join the group",
        ParameterSetName = "UserId"
        )]
        public int? UserId { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "Email of a user to join the group. If the email does not correspond to an existing user then the user will be invited to join the network too (if you are network admin).",
        ParameterSetName = "Email"
        )]
        public string Email { get; set; }

        protected override void ProcessRecord()
        {
            var token = SessionState.PSVariable.Get(Properties.Resources.TokenVariable);
            if (token == null)
            {
                WriteWarning(Properties.Resources.EmptyTokenWarning);
                return;
            }
            _request = new Request(token.Value.ToString());

            string email = string.Empty;
            string userId = string.Empty;

            if (Email != null)
            {
                email = "&email=" + Email;
            }
            if (UserId != null)
            {
                userId = "&user_id=" + UserId;
            }

            try
            {
                var url = string.Format("{0}group_memberships.json?group_id={1}{2}{3}", Properties.Resources.YammerApi, GroupId, userId, email);
                _request.Post(url, string.Empty);
            }
            catch (Exception e)
            {
                var errorRecord = new ErrorRecord(e, "e", ErrorCategory.InvalidArgument, "Arguments invalid");
                WriteError(errorRecord);
            }
        }

    }
}
