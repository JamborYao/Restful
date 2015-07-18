﻿using System;
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
    public  class listContainer
    {
        string _accountName="";
        string _accountKey;
        string _listContainerUrl = string.Format("https://{0}.blob.core.windows.net/mycontainer?restype=container&comp=list", "danielfiletest");
        string _xdate = DateTime.UtcNow.ToString("R");
        string _xversion = "2014-02-14";
        string _method = "GET";
        string _containerName = "mycontainer";

        public listContainer(string accountName,string accountKey)
        {
            this._accountName = accountName;
            this._accountKey = accountKey;
        }
        public void HttpListContainer()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(_listContainerUrl));
            request.Method = _method;
            request.Headers.Add("x-ms-date", _xdate);
            request.Headers.Add("x-ms-version", _xversion);
            string headerresource = string.Format("x-ms-date:{0}\nx-ms-version:{1}", _xdate, _xversion);
            string urlresource = string.Format("/{0}/{1}\ncomp:list\nrestype:container",_accountName,_containerName);
            string stringtosign = string.Format("{0}\n\n\n\n\n\n\n\n\n\n\n\n{1}\n{2}",_method, headerresource, urlresource);
            string authorization = SignToString(stringtosign);

            request.Headers.Add("Authorization", authorization);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
               var stream= response.GetResponseStream();
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
