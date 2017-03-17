using System.Management.Automation;

namespace YammerShell
{
    [Cmdlet(VerbsCommon.Set, "YmToken")]
    public class SetYmToken : PSCmdlet
    {
        [Parameter(
        Mandatory = true,
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        Position = 0,
        HelpMessage = "Bearer token needed to authorize."
        )]
        public string Token { get; set; }

        protected override void ProcessRecord()
        {
            PSVariable token = new PSVariable(Properties.Resources.TokenVariable, Token);
            SessionState.PSVariable.Set(token);
        }
    }
}
