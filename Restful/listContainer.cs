using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security.Cryptography;

namespace Restful
{
    public  class listContainer
    {
        string _accountName;
        string _accountKey;
        string _listContainerUrl = "";
        string _xdate = DateTime.UtcNow.ToString("R");
        string xversion = "2014-02-14";

        public listContainer(string accountName,string accountKey)
        {
            this._accountName = accountName;
            this._accountKey = accountKey;
        }
        public void ListContainer()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(_listContainerUrl));
            request.Headers.Add("x-ms-date", _xdate);
            request.Headers.Add("x-ms-version", xversion);
            request.Headers.Add("Authorization", "");
        }
        public string SignToString() 
        {

            HMACSHA256 hmac = new HMACSHA256(Convert.FromBase64String(_accountKey));
            string authorization= Convert.ToBase64String( hmac.ComputeHash(Encoding.UTF8.GetBytes("")));
            return authorization;
        }

    }
}
