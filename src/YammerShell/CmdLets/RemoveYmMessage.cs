using System;
using System.Management.Automation;

namespace YammerShell.CmdLets
{
    [Cmdlet(VerbsCommon.Remove, "YmMessage")]
    public class RemoveYmMessage : PSCmdlet
    {
        private Request _request;

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        Mandatory = true,
        Position = 0,
        HelpMessage = "ID of the message to delete"
        )]
        public long Id { get; set; }

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
                _request.Delete(string.Format("{0}messages/{1}", Properties.Resources.YammerApi, Id));
            }
            catch (Exception e)
            {
                var errorRecord = new ErrorRecord(e, "94", ErrorCategory.InvalidArgument, Id);
                WriteError(errorRecord);
            }
        }
    }
}
