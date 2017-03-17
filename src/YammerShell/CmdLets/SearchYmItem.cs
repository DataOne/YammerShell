using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using YammerShell.YammerObjects;

namespace YammerShell.CmdLets
{
    [Cmdlet(VerbsCommon.Search, "YmItem")]
    public class SearchYmItem : PSCmdlet
    {
        private Request _request;

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        Position = 0,
        Mandatory = true,
        HelpMessage = "Term to search"
        )]
        public string SearchItem { get; set; }

        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "Number of the result page (max 20 items per page)"
        )]
        public int Page { get; set; } = 1;

        [ValidateRange(1, 20)]
        [Parameter(
        ValueFromPipelineByPropertyName = true,
        ValueFromPipeline = true,
        HelpMessage = "Maximum of results for each page (between 1 and 20)."
        )]
        public int Limit { get; set; } = 20;

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
                var url = string.Format("{0}search.json?search={1}&num_per_page={2}&page={3}", Properties.Resources.YammerApi, SearchItem, Limit, Page);
                var response = JObject.Parse(_request.Get(url));

                var searchResult = GetYammerSearchResult(response);
                searchResult.Page = Page;
                WriteObject(searchResult);
            }
            catch (Exception e)
            {
                var errorRecord = new ErrorRecord(e, "533", ErrorCategory.InvalidOperation, SearchItem);
                WriteError(errorRecord);
            }
        }

        private YammerSearchResult GetYammerSearchResult(JObject response)
        {
            var yammerSearchResult = new YammerSearchResult();

            var count = response["count"];
            yammerSearchResult.TotalMessages = Convert.ToInt32(count["messages"]);
            yammerSearchResult.TotalGroups = Convert.ToInt32(count["groups"]);
            yammerSearchResult.TotalTopics = Convert.ToInt32(count["topics"]);
            yammerSearchResult.TotalFiles = Convert.ToInt32(count["uploaded_files"]);
            yammerSearchResult.TotalUsers = Convert.ToInt32(count["users"]);

            var messages = new List<YammerMessage>();
            foreach (var message in JArray.Parse(response["messages"]["messages"].ToString()))
            {
                messages.Add(GetYammerMessage(message));
            }
            yammerSearchResult.Messages = messages;

            var groups = new List<YammerGroup>();
            foreach (var group in JArray.Parse(response["groups"].ToString()))
            {
                groups.Add(GetYammerGroup(group));
            }
            yammerSearchResult.Groups = groups;

            var topics = new List<YammerTopic>();
            foreach (var topic in JArray.Parse(response["topics"].ToString()))
            {
                topics.Add(GetYammerTopic(topic));
            }
            yammerSearchResult.Topics = topics;

            var files = new List<YammerFile>();
            foreach (var file in JArray.Parse(response["uploaded_files"].ToString()))
            {
                files.Add(GetYammerFile(file));
            }
            yammerSearchResult.Files = files;

            var users = new List<YammerUser>();
            foreach (var user in JArray.Parse(response["users"].ToString()))
            {
                users.Add(GetYammerUser(user));
            }
            yammerSearchResult.Users = users;

            return yammerSearchResult;
        }

        private YammerUser GetYammerUser(JToken user)
        {
            var yammerUser = new YammerUser();
            yammerUser.Id = Convert.ToInt32(GetToken(user, "id"));
            yammerUser.UserName = GetToken(user, "name");
            yammerUser.FirstName = GetToken(user, "first_name");
            yammerUser.LastName = GetToken(user, "last_name");
            yammerUser.FullName = GetToken(user, "full_name");
            yammerUser.Email = GetToken(user, "email");
            yammerUser.JobTitle = GetToken(user, "job_title");
            yammerUser.Department = GetToken(user, "department");
            yammerUser.Timezone = GetToken(user, "timezone");
            yammerUser.NetworkId = Convert.ToInt32(GetToken(user, "network_id"));
            yammerUser.NetworkName = GetToken(user, "network_name");
            yammerUser.Url = GetToken(user, "web_url");
            var activatedAt = user["activated_at"];
            yammerUser.ActivatedAt = activatedAt.Type == JTokenType.Null ? DateTime.MinValue : (DateTime)user["activated_at"];

            var phoneNumbers = new List<string>();
            foreach (var number in new JArray(user["phone_numbers"]).Children())
            {
                phoneNumbers.Add(number.ToString());
            }
            yammerUser.PhoneNumbers = phoneNumbers;

            return yammerUser;
        }

        private string GetToken(JToken token, string key)
        {
            var value = token[key];
            if (value == null)
            {
                return "";
            }
            return value.ToString();
        }

        private YammerFile GetYammerFile(JToken file)
        {
            var yammerFile = new YammerFile();

            yammerFile.Id = Convert.ToInt32(file["id"]);
            yammerFile.NetworkId = Convert.ToInt32(file["network_id"]);
            yammerFile.GroupId = file.Value<int?>("group_id");
            yammerFile.OwnerId = file.Value<int?>("owner_id");
            yammerFile.Url = file["web_url"].ToString();
            yammerFile.Name = file["name"].ToString();
            yammerFile.Description = file["description"].ToString();
            yammerFile.ContentClass = file["content_class"].ToString();
            yammerFile.CreatedAt = (DateTime)file["created_at"];
            yammerFile.Privacy = file["privacy"].ToString();

            return yammerFile;
        }

        private YammerTopic GetYammerTopic(JToken topic)
        {
            var yammerTopic = new YammerTopic();
            yammerTopic.Id = Convert.ToInt32(topic["id"]);
            yammerTopic.Name = topic["name"].ToString();
            yammerTopic.NetworkId = Convert.ToInt32(topic["network_id"]);
            yammerTopic.Url = topic["web_url"].ToString();
            return yammerTopic;
        }

        private YammerGroup GetYammerGroup(JToken group)
        {
            var yammerGroup = new YammerGroup();
            yammerGroup.Id = Convert.ToInt32(group["id"]);
            yammerGroup.Email = group["email"].ToString();
            yammerGroup.FullName = group["full_name"].ToString();
            yammerGroup.Name = group["name"].ToString();
            yammerGroup.NetworkId = Convert.ToInt32(group["network_id"]);
            yammerGroup.Description = group["description"].ToString();
            yammerGroup.Privacy = group["privacy"].ToString();
            yammerGroup.Url = group["web_url"].ToString();
            yammerGroup.CreatedAt = (DateTime)group["created_at"];
            yammerGroup.CreatorId = Convert.ToInt32(group["creator_id"]);
            yammerGroup.Members = Convert.ToInt32(group["stats"]["members"]);
            return yammerGroup;
        }

        private YammerMessage GetYammerMessage(JToken message)
        {
            var yammerMessage = new YammerMessage();
            yammerMessage.Id = Convert.ToInt32(message["id"]);
            yammerMessage.SenderId = Convert.ToInt32(message["sender_id"]);
            yammerMessage.RepliedToId = message.Value<int?>("replied_to_id");
            yammerMessage.CreatedAt = (DateTime)message["created_at"];
            yammerMessage.NetworkId = Convert.ToInt32(message["network_id"]);
            yammerMessage.MessageType = message["message_type"].ToString();
            yammerMessage.GroupId = message.Value<int?>("group_id");
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
