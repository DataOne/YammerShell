using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using YammerShell.YammerObjects;

namespace YammerShell.CmdLets
{
    [Cmdlet(VerbsCommon.Get, "YmMessage", DefaultParameterSetName = "Id")]
    public class GetYmMessage : PSCmdlet
    {
        private Request _request;

        [Parameter(
        HelpMessage = "Return top messages",
        ParameterSetName = "Network"
        )]
        public SwitchParameter Top { get; set; }

        [Parameter(
        HelpMessage = "Return messages of followed users",
        ParameterSetName = "Network"
        )]
        public SwitchParameter Following { get; set; }

        [Parameter(
        HelpMessage = "Return all messages received by the user",
        ParameterSetName = "ReceivedPrivate"
        )]
        public SwitchParameter Received { get; set; }

        [Parameter(
        HelpMessage = "Return all private messages received by the user",
        ParameterSetName = "ReceivedPrivate"
        )]
        public SwitchParameter Private { get; set; }

        [Parameter(
        HelpMessage = "Return all private messages sent by the user",
        ParameterSetName = "Sent"
        )]
        public SwitchParameter Sent { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        Position = 0,
        HelpMessage = "Id of a message",
        ParameterSetName = "Id"
        )]
        public string Id { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "Limit of messages"
        )]
        public int? Limit { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "Returns messages older than this message ID"
        )]
        public string OlderThan { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "The feed of messages for a hashtag specified by the numeric string ID",
        ParameterSetName = "Topic"
        )]
        public string Topic { get; set; }

        protected override void ProcessRecord()
        {
            var token = SessionState.PSVariable.Get(Properties.Resources.TokenVariable);
            if (token == null)
            {
                WriteWarning(Properties.Resources.EmptyTokenWarning);
                return;
            }
            _request = new Request(token.Value.ToString());

            if (!string.IsNullOrEmpty(Id))
            {
                try
                {
                    var url = string.Format("{0}messages/{1}.json", Properties.Resources.YammerApi, Id);
                    var response = JToken.Parse(_request.Get(url));
                    WriteObject(GetYammerMessage(response));
                }
                catch (Exception e)
                {
                    var errorRecord = new ErrorRecord(e, "id", ErrorCategory.InvalidArgument, Id);
                    WriteError(errorRecord);
                }
                return;
            }
            if (Sent.IsPresent || Received.IsPresent || Private.IsPresent)
            {
                WriteObject(GetMessages(), true);
            }
            else
            {
                WriteObject(GetNetworkMessages(), true);
            }
        }

        private IEnumerable<YammerMessage> GetMessages()
        {
            string requestUrl;
            string parameters = string.Empty;

            if (Limit != null)
            {
                parameters = "?limit=" + Limit;
                if (OlderThan != null)
                {
                    parameters += "&older_than=" + OlderThan;
                }
            }
            else
            {
                if (OlderThan != null)
                {
                    parameters = "?older_than=" + OlderThan;
                }
            }
            if (Private.IsPresent)
            {
                requestUrl = Properties.Resources.YammerApi + "messages/private.json";
                return GetMessagesFromApi(requestUrl, parameters);
            }
            if (Received.IsPresent)
            {
                requestUrl = Properties.Resources.YammerApi + "messages/received.json";
                return GetMessagesFromApi(requestUrl, parameters);
            }
            requestUrl = Properties.Resources.YammerApi + "messages/sent.json";
            return GetMessagesFromApi(requestUrl, parameters);
        }

        public IEnumerable<YammerMessage> GetNetworkMessages()
        {
            string requestUrl;
            string parameters = string.Empty;

            if (Limit != null)
            {
                parameters = "?limit=" + Limit;
                if (OlderThan != null)
                {
                    parameters += "&older_than=" + OlderThan;
                }
            }
            else
            {
                if (OlderThan != null)
                {
                    parameters = "?older_than=" + OlderThan;
                }
            }

            if (!string.IsNullOrEmpty(Topic))
            {
                requestUrl = Properties.Resources.YammerApi + "messages/about_topic/" + Topic + ".json";
                return GetMessagesFromApi(requestUrl, parameters);
            }
            if (Top.IsPresent && Following.IsPresent)
            {
                requestUrl = Properties.Resources.YammerApi + "messages/my_feed.json";
                return GetMessagesFromApi(requestUrl, parameters);
            }
            if (Top.IsPresent)
            {
                requestUrl = Properties.Resources.YammerApi + "messages/algo.json";
                return GetMessagesFromApi(requestUrl, parameters);
            }
            if (Following.IsPresent)
            {
                requestUrl = Properties.Resources.YammerApi + "messages/following.json";
                return GetMessagesFromApi(requestUrl, parameters);
            }
            requestUrl = Properties.Resources.YammerApi + "messages.json";
            return GetMessagesFromApi(requestUrl, parameters);
        }

        private IEnumerable<YammerMessage> GetMessagesFromApi(string requestUrl, string parameters)
        {
            var allMessages = new List<YammerMessage>();
            try
            {
                var result = JObject.Parse(_request.Get(requestUrl + parameters));
                var messages = JArray.Parse(result["messages"].ToString());

                foreach (var message in messages)
                {
                    allMessages.Add(GetYammerMessage(message));
                }
            }
            catch (Exception e)
            {
                var errorRecord = new ErrorRecord(e, "403", ErrorCategory.InvalidArgument, "Arguments invalid");
                WriteError(errorRecord);
            }
            return allMessages;
        }

        private YammerMessage GetYammerMessage(JToken message)
        {
            var yammerMessage = new YammerMessage();
            yammerMessage.Id = message["id"].ToString();
            yammerMessage.SenderId = message["sender_id"].ToString();
            yammerMessage.RepliedToId = message.Value<string>("replied_to_id");
            yammerMessage.CreatedAt = (DateTime)message["created_at"];
            yammerMessage.NetworkId = message["network_id"].ToString();
            yammerMessage.MessageType = message["message_type"].ToString();
            yammerMessage.GroupId = message.Value<string>("group_id");
            yammerMessage.Body = message["body"]["plain"].ToString();
            yammerMessage.Url = message["web_url"].ToString();

            var attachments = new List<string>();
            foreach (var attachment in JArray.Parse(message["attachments"].ToString()))
            {
                attachments.Add(attachment["web_url"].ToString());
            }
            yammerMessage.AttachmentUrls = attachments;

            return yammerMessage;
        }
    }
}
