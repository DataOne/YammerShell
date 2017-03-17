using System.Management.Automation;

namespace YammerShell.CmdLets
{
    [Cmdlet(VerbsCommon.Show, "YmToken")]
    public class ShowYmToken : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            var token = SessionState.PSVariable.Get(Properties.Resources.TokenVariable);
            if (token == null)
            {
                WriteWarning(Properties.Resources.EmptyTokenWarning);
                return;
            }
            WriteObject(token.Value);
        }
    }
}
