using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security.Cryptography;
using System.Globalization;
using System.IO;

namespace Restful
{
    public class ListBlock
    {
        string _accountName;
        string _accountKey;
        string _listContainerUrl;
        string _xdate;
        string _xversion;
        string _method;
        string _containerName;
        string _blobName;

        public ListBlock(string accountName, string accountKey, string containerName,string blobName)
        {
            this._accountName = accountName;
            this._accountKey = accountKey;
            this._containerName = containerName;
            this._blobName = blobName;
            this._listContainerUrl = string.Format(
                "https://{0}.blob.core.windows.net/{1}/{2}?comp=blocklist&blocklisttype=all",
                this._accountName,
                this._containerName,
                this._blobName
                );
            this._method = "GET";
            this._xdate = DateTime.UtcNow.ToString("R");
            this._xversion = "2014-02-14";
        }
        public void HttpListBlock()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(_listContainerUrl));
            request.Method = _method;
            request.Headers.Add("x-ms-date", _xdate);
            request.Headers.Add("x-ms-version", _xversion);
            string headerresource = string.Format("x-ms-date:{0}\nx-ms-version:{1}", _xdate, _xversion);
            string urlresource = string.Format("/{0}/{1}/{2}\nblocklisttype:all\ncomp:blocklist", _accountName, _containerName, _blobName);
            string stringtosign = string.Format("{0}\n\n\n\n\n\n\n\n\n\n\n\n{1}\n{2}", _method, headerresource, urlresource);
            string authorization = SignToString(stringtosign);

            request.Headers.Add("Authorization", authorization);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                var stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                string content = reader.ReadToEnd();
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
