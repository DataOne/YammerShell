using Newtonsoft.Json.Linq;
using System;
using System.Management.Automation;

namespace YammerShell.CmdLets
{
    [Cmdlet(VerbsCommon.New, "YmUser")]
    public class NewYmUser : PSCmdlet
    {
        private Request _request;

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        Mandatory = true,
        Position = 0,
        HelpMessage = "Email of the new user"
        )]
        public string Email { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "Full name of the new user"
        )]
        public string FullName { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "Job title of the new user"
        )]
        public string JobTitle { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "Department name of the new user"
        )]
        public string DepartmentName { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "Location of the new user"
        )]
        public string Location { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "Work phone number of the new user"
        )]
        public string WorkTelephone { get; set; }


    protected override void ProcessRecord()
        {
            var token = SessionState.PSVariable.Get(Properties.Resources.TokenVariable);
            if (token == null)
            {
                WriteWarning(Properties.Resources.EmptyTokenWarning);
                return;
            }
            _request = new Request(token.Value.ToString());

            string fullName = string.Empty;
            string jobTitle = string.Empty;
            string departmentName = string.Empty;
            string location = string.Empty;
            string workTelephone = string.Empty;

            if (FullName != null)
            {
                fullName = "&full_name=" + FullName;
            }
            if (jobTitle != null)
            {
                jobTitle = "&job_title=" + JobTitle;
            }
            if (DepartmentName != null)
            {
                departmentName = "&department_name=" + DepartmentName;
            }
            if (Location != null)
            {
                location = "&location=" + Location;
            }
            int ignore;
            if (WorkTelephone != null && int.TryParse(WorkTelephone, out ignore))
            {
                workTelephone = "&work_telephone=" + WorkTelephone;
            }
            else
            {
                var errorRecord = new ErrorRecord(new ArgumentException(), WorkTelephone, ErrorCategory.InvalidArgument, WorkTelephone);
                WriteError(errorRecord);
                return;
            }

            try
            {
                // TODO test as admin if new user gets created and id is returned
                var url = string.Format("{0}users.json?{1}{2}{3}{4}{5}{6}", Properties.Resources.YammerApi, Email, fullName, jobTitle, departmentName, location, workTelephone);
                var response = _request.Post(url, string.Empty);
                var newUser = JObject.Parse(response);
                WriteObject(Convert.ToInt64(newUser["id"]));
            }
            catch (Exception e)
            {
                var errorRecord = new ErrorRecord(e, "X", ErrorCategory.InvalidArgument, Email);
                WriteError(errorRecord);
            }
        }
    }
}
