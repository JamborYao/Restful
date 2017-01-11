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
    public class AzureTable
    {
        static string StorageAccount = "fehanstoragetest";
        static string  StorageKey = "d5qSSNsS1ic3RuLCJ52p8KMP+GyrtIF4S8TftjN51ndd7OuIhMWlaunvoBbNJ5/ISW3nD5lfagVOsVopqMm4lA==";

        static string tablename = "Employee";

        static string requestMethod = "DELETE";
        static string mxdate = DateTime.UtcNow.ToString("R");
        //string storageServiceVersion = "2015-04-05";
        static string storageServiceVersion = "2012-02-12";

        public static void DeleteTable()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(string.Format(CultureInfo.InvariantCulture,
                "https://{0}.table.core.windows.net/Tables('{1}')",
                StorageAccount, tablename));

            req.Method = requestMethod;

            //specify request header
            string AuthorizationHeader = generateAuthorizationHeader();
            req.Headers.Add("Authorization", AuthorizationHeader);
            req.Headers.Add("x-ms-date", mxdate);
            req.Headers.Add("x-ms-version", storageServiceVersion);
            req.ContentType = "application/xml";

            //req.Accept = "application/atom+xml,application/xml";
            //req.Headers.Add("DataServiceVersion", "2.0;NetFx");
            //req.Headers.Add("MaxDataServiceVersion", "2.0;NetFx");

            //req.ContentType = "application/xml";

            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            {

            }
        }

        public static string generateAuthorizationHeader()
        {
            

            string canonicalizedResource = ($"/{StorageAccount}/Tables('{tablename}')"); //string.Format("/{0}\nTables('{1}')", StorageAccount, tablename);


            string contentMD5 = String.Empty;
            string contentType =("application/xml");

            //string canonicalizedResource = String.Format("/{0}/{1}", StorageAccount, "https://fehanstoragetest.table.core.windows.net/NewClass");

            string stringToSign = $"{requestMethod}\n\n{contentType}\n{mxdate}\n{canonicalizedResource}";

            HMACSHA256 hmac = new HMACSHA256(Convert.FromBase64String(StorageKey));

            string signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));

            String authorization = String.Format("{0} {1}:{2}",
              "SharedKey",
              StorageAccount,
              signature
              );

            return authorization;
        }

    }
}
