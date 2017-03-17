using System;
using System.Management.Automation;

namespace YammerShell.CmdLets
{
    [Cmdlet(VerbsCommon.Remove, "YmGroupMembership", DefaultParameterSetName = "UserId")]
    public class RemoveYmGroupMembership : PSCmdlet
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
        HelpMessage = "ID of the user to leave the group",
        ParameterSetName = "UserId"
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
            
            string userId = string.Empty;
            if (UserId != null)
            {
                userId = "&user_id=" + UserId;
            }

            try
            {
                var url = string.Format("{0}group_memberships.json?group_id={1}{2}", Properties.Resources.YammerApi, GroupId, userId);
                _request.Delete(url);
            }
            catch (Exception e)
            {
                var errorRecord = new ErrorRecord(e, "e", ErrorCategory.InvalidArgument, "Arguments invalid");
                WriteError(errorRecord);
            }
        }

    }
}
