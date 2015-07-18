using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Restful
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri("https://danielfiletest.blob.core.windows.net/mycontainer?restype=container&comp=list"));
            request.Method = "GET";
            request.Headers.Add("x-ms-version", "2014-02-14");
            /*
             身份验证大体分为：基于共享密钥的身份验证、基于生物学特征的身份验证和基于公开密钥加密算法的身份验证。
             基于共享密钥的身份验证是指服务器端和用户共同拥有一个或一组密码
             */
            string dateString = DateTime.UtcNow.ToString("R");
            request.Headers.Add("x-ms-date", dateString);
            request.Headers.Add("Authorization", getSignature("NntZ4wYSLxyqxLJXTJUSCUscmZdO6BKN0KmZQU7pIUd31dba/k38T1jYimo+TuVy1EFrlJURWPFJ23RALur0Bw==", dateString));
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
            }

        }

        public static string getSignature(string key, string dateString)
        {

            string nowDate = DateTime.UtcNow.ToString("R", System.Globalization.CultureInfo.InvariantCulture);
            String canonicalizedHeaders = String.Format(
            "x-ms-date:{0}\nx-ms-version:{1}",
            dateString,
            "2014-02-14");
            String canonicalizedResource = "/danielfiletest/mycontainer\nrestype:container\ncomp:list";

            String stringToSign = String.Format(
            "{0}\n" +
            "\n" +
            "\n" +
            "\n" +
            "\n" +
            "\n" +
            "\n" +
            "\n" +
            "\n" +
            "\n" +
            "\n" +
            "\n" +
            "{1}\n" +
            "{2}",
            "GET", canonicalizedHeaders, 
            "/danielfiletest/mycontainer\ncomp:list\nrestype:container"
            //canonicalizedResource
            );


            HMACSHA256 hmac = new HMACSHA256(Convert.FromBase64String(key));
            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));

            String authorizationHeader = String.Format(CultureInfo.InvariantCulture, "{0} {1}:{2}",
                "SharedKey",
                "danielfiletest",
                signature
                );
            return authorizationHeader;
        }

    }
}
