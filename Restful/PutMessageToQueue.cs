using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;



namespace Restful
{
    public class PutMessageToQueue
    {
        string _accountName;
        string _accountKey;
        string _url;
        string _xdate;
        string _xversion;
        string _method;
        string _queueName;

        public PutMessageToQueue(string accountName, string accountKey, string queueName)
        {
            this._accountName = accountName;
            this._accountKey = accountKey;
            this._queueName = queueName;
            this._url = string.Format(
                "https://{0}.queue.core.windows.net/{1}/messages?visibilitytimeout={2}&messagettl={3}",
                this._accountName,
                this._queueName,0,3600*24*5);
            this._method = "POST";
            this._xdate = DateTime.UtcNow.ToString("R");
            this._xversion = "2014-02-14";
        }
        public void SendMessage()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(_url));
            request.Method = _method;
            request.Headers.Add("x-ms-date", _xdate);
            request.Headers.Add("x-ms-version", _xversion);

            string requestContent = "<QueueMessage>" +
                                      "<MessageText>hello world</MessageText>" +
                                  "</QueueMessage>";
            // request.GetRequestStream()
            string data=System.Web.HttpUtility.UrlEncode(requestContent);
            byte[] bytes=Encoding.UTF8.GetBytes(data);
            request.ContentLength = bytes.Length;
            using (var stream = request.GetRequestStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(Convert.ToBase64String( Encoding.UTF8.GetBytes(System.Web.HttpUtility.UrlEncode(requestContent))));
            }
            string headerresource = string.Format("x-ms-date:{0}\nx-ms-version:{1}", _xdate, _xversion);
            string urlresource = string.Format("/{0}/{1}/messages\nmessagettl:{2}\nvisibilitytimeout:0",
                _accountName, _queueName,3600*24*5);
            string stringtosign = string.Format("{0}\n\n\n{1}\n\n\n\n\n\n\n\n\n{2}\n{3}", _method, bytes.Length, headerresource, urlresource);
            string authorization = SignToString(stringtosign);

          

            request.Headers.Add("Authorization", authorization);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                // Stream stream = response.GetResponseStream();
                HttpStatusCode code = response.StatusCode;

            }
        }
        public string SignToString(string stringtosign)
        {

            HMACSHA256 hmac = new HMACSHA256(Convert.FromBase64String(_accountKey));
            string signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringtosign)));
            String authorization = String.Format("{0} {1}:{2}",
              "SharedKey",
             _accountName,
              signature
              );
            return authorization;
        }
    }
}
