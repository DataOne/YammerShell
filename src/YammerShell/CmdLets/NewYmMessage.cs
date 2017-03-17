using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace YammerShell.CmdLets
{
    [Cmdlet(VerbsCommon.New, "YmMessage")]
    public class NewYmMessage : PSCmdlet
    {
        private Request _request;

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "ID of an existing conversation",
        ParameterSetName = "Replied"
        )]
        public int? RepliedToId { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "ID of a yammer group",
        ParameterSetName = "Group"
        )]
        public int? GroupId { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "Send a private message to one or more users, specified by ID",
        ParameterSetName = "Private"
        )]
        public int[] DirectToUserIds { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        Mandatory = true,
        Position = 0,
        HelpMessage = "Body of the message"
        )]
        public string Body { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "Set that the message is of type announcement and set the title of the announcement",
        ParameterSetName = "Announcement"
        )]
        [Parameter(ParameterSetName = "Group")]
        public string AnnouncementTitle { get; set; }

        protected override void ProcessRecord()
        {
            var token = SessionState.PSVariable.Get(Properties.Resources.TokenVariable);
            if (token == null)
            {
                WriteWarning(Properties.Resources.EmptyTokenWarning);
                return;
            }
            _request = new Request(token.Value.ToString());

            string directToUsers = string.Empty;
            string repliedTo = string.Empty;
            string group = string.Empty;
            string announcement = string.Empty;

            if (GroupId != null)
            {
                group = "&group_id=" + GroupId;
            }
            if (DirectToUserIds != null)
            {
                directToUsers = "&direct_to_user_ids=" + string.Join(",", DirectToUserIds.Select(i => i.ToString()).ToArray());
            }
            if (AnnouncementTitle != null)
            {
                announcement = "&is_rich_text=true&message_type=announcement&title=" + AnnouncementTitle;
            }
            if (RepliedToId != null)
            {
                repliedTo = "&replied_to_id=" + RepliedToId;
            }

            try
            {
                var response = _request.Post(string.Format("{0}messages.json?body={1}{2}{3}{4}{5}", Properties.Resources.YammerApi, Body, repliedTo, directToUsers, group, announcement), string.Empty);
                var jObject = JObject.Parse(response);
                var messages = JArray.Parse(jObject["messages"].ToString());
                var id = messages[0]["id"];
                WriteObject(Convert.ToInt32(id));
            }
            catch (Exception e)
            {
                var errorRecord = new ErrorRecord(e, "42", ErrorCategory.InvalidArgument, "Arguments invalid");
                WriteError(errorRecord);
            }
        }

    }
}
