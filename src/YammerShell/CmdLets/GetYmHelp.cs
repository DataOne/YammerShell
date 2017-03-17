using System.Management.Automation;

namespace YammerShell.CmdLets
{
    [Cmdlet(VerbsCommon.Get, "YmHelp")]
    public class GetYmHelp : PSCmdlet
    {
        [Parameter(
        HelpMessage = "You should better be polite..."
        )]
        public SwitchParameter Force { get; set; }

        protected override void ProcessRecord()
        {
            if (Force.IsPresent)
            {
                WriteObject("\nYou can not force YammerShell to help you.\n");
                return;
            }

            var helpMessage = "\n"
                            + "-Add-YmGroupMembership: add a user to a group \n"
                            + "-Get-YmGroup: returns all groups or a specific group selected by id \n"
                            + "-Get-YmMessage: shows messages of the user's Yammer network \n"
                            + "-Get-YmNetwork: returns a list of networks to which the current user has access \n"
                            + "-Get-YmToken: navigates you through the pages which are needed to get the bearer token \n"
                            + "-Get-YmUser: returns all users in the user's Yammer network or a specific user selected by username \n"
                            + "-New-YmGroup: creates a new group in yammer \n"
                            + "-New-YmMessage: posts a new message or announcement to the network or a group \n"
                            + "-New-YmUser: Creates a new user. Current user should be a verified admin to perform this action \n"
                            + "-Remove-Ym-GroupMembership: remove a user from a group \n"
                            + "-Remove-YmMessage: deletes a message per id \n"
                            + "-Search-YmItem: search messages, users, topics and groups \n"
                            + "-Send-YmInvitation: send an invitation to the yammer network \n"
                            + "-Set-YmToken: sets the bearer token needed to access Yammer \n"
                            + "-Show-YmToken: shows the currently set token \n";
            WriteObject(helpMessage);
        }
    }
}
