using Newtonsoft.Json.Linq;
using System;
using System.Management.Automation;

namespace YammerShell
{
    [Cmdlet(VerbsCommon.Get, "YmToken")]
    public class GetYmToken : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("First you have to register an application. Press any key to open 'https://www.yammer.com/client_applications' \n");
            Console.ReadKey(true);
            System.Diagnostics.Process.Start("https://www.yammer.com/client_applications");

            WriteObject("Click on 'Register new app' and fill the blanks. You have to set the redirect uri to 'http://www.google.de'. "
                + "After your app has been registered you need to copy the CLIENT ID and paste it here: ");
            var clientId = Console.ReadLine().Trim();

            WriteObject("Now copy the CLIENT SECRET and paste it: ");
            var clientSecret = Console.ReadLine().Trim();

            var url = "https://www.yammer.com/oauth2/authorize?client_id=" + clientId + "&response_type=code&redirect_uri=https://www.google.de";
            WriteObject("\nTo receive an authorization code press any key to open '" + url + "'");
            Console.ReadKey(true);
            System.Diagnostics.Process.Start(url);

            WriteObject("\nYou get redirected and have to click on 'Allow'. This redirects you to 'https://www.google.de/?code=[secret]' where [secret] is the code you need to copy and paste here: ");
            var code = Console.ReadLine().Trim();

            var tokenUrl = string.Format("https://www.yammer.com/oauth2/access_token?client_id={0}&client_secret={1}&code={2}", clientId, clientSecret, code);
            var request = new Request("");
            try
            {
                var accessToken = JObject.Parse(request.Post(tokenUrl, string.Empty));

                var bearer = accessToken["access_token"]["token"].ToString();
                var user = accessToken["user"]["name"].ToString();
                var network = accessToken["network"]["name"].ToString();
                
                PSVariable token = new PSVariable(Properties.Resources.TokenVariable, bearer);
                SessionState.PSVariable.Set(token);
                WriteObject(string.Format("\nBearer token received and set for user '{0}' in Yammer network '{1}'. To show it type: Show-YmToken \n", user, network));
            }
            catch (Exception e)
            {
                var errorRecord = new ErrorRecord(e, "0", ErrorCategory.PermissionDenied, tokenUrl);
                WriteError(errorRecord);
            }
        }
    }
}
