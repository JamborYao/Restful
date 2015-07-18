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
    public class CreateContainer
    {
        string _accountName;
        string _accountKey;
        string _listContainerUrl = "https://danielfiletest.blob.core.windows.net/jambor?restype=container";
        string _xdate = DateTime.UtcNow.ToString("R");
        string _xversion = "2011-08-18";
        string _method = "PUT";
        string _containerName = "jambor";

        public CreateContainer(string accountKey,string accountName)
        {
            this._accountKey = accountKey;
            this._accountName = accountName;
        }
        public void CallCreateContainer()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(_listContainerUrl));
            request.Method = _method;
            request.Headers.Add("x-ms-date", _xdate);
            request.Headers.Add("x-ms-version", _xversion);
            request.ContentLength = 0;
            string headerresource = string.Format("x-ms-date:{0}\nx-ms-version:{1}", _xdate, _xversion);
            string urlresource = string.Format("/{0}/{1}\nrestype:container", _accountName, _containerName);
            string stringtosign = string.Format("{0}\n\n\n{1}\n\n\n\n\n\n\n\n\n{2}\n{3}", _method,0, headerresource, urlresource);
            string authorization = SignToString(stringtosign);
            
            // request.GetRequestStream()

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
