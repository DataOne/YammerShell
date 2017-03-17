using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace YammerShell
{
    public class Request
    {
        public Request(string token)
        {
            _token = token;
        }
        private string _token;

        public string Get(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = WebRequestMethods.Http.Get;
            request.Headers.Add("Authorization", "Bearer " + _token);

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            string result;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                result = reader.ReadToEnd();
            }
            response.Close();

            return result;
        }

        public string Post(string url, string postData)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = WebRequestMethods.Http.Post;
            request.Headers.Add("Authorization", "Bearer " + _token);

            if (postData.StartsWith("{"))
            {
                request.ContentType = "application/json";
            }
            else
            {
                request.ContentType = "x-www-form-urlencoded";
                postData = HttpUtility.UrlPathEncode(postData);
            }
            byte[] postBytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = postBytes.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(postBytes, 0, postBytes.Length);
            }

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            string result;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                result = reader.ReadToEnd();
            }
            response.Close();

            return result;
        }

        public void Delete(string url)
        {
            HttpWebRequest request = WebRequest.Create(HttpUtility.UrlPathEncode(url)) as HttpWebRequest;
            request.Method = "DELETE";
            request.Headers.Add("Authorization", "Bearer " + _token);

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            response.Close();
        }
        
    }
}
