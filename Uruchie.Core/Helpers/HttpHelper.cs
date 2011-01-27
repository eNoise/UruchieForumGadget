using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Uruchie.Core.Helpers
{
    public static class HttpHelper
    {
        /// <summary>
        /// Send POST
        /// </summary>
        public static string HttpPost(string uri, string parameters)
        {
            WebRequest req = WebRequest.Create(uri);
            //req.Proxy = new System.Net.WebProxy(ProxyString, true);
            //Add these, as we're doing a POST
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            //We need to count how many bytes we're sending. Post'ed Faked Forms should be name=value&
            byte[] bytes = Encoding.UTF8.GetBytes(parameters);
            req.ContentLength = bytes.Length;
            Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();
            WebResponse resp = req.GetResponse();
            if (resp == null) return null;
            var sr = new StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        public static string Encode(string str)
        {
            //todo: replace with own implementation due to dependence of System.Web, which is not exists in ".NET Client Profile"
            return HttpUtility.HtmlEncode(str);
        }

        public static string Decode(string str)
        {
            return HttpUtility.HtmlDecode(str);
        }
    }
}