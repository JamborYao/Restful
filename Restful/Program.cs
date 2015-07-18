using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Restful
{
    class Program
    {
        static void Main(string[] args)
        {
            string accountName = "danielfiletest";
            string accountKey = ConfigurationManager.AppSettings.Get(accountName);
            listContainer listContainer = new listContainer(accountName, accountKey);
            listContainer.HttpListContainer();

            CreateContainer createContainer = new CreateContainer(accountKey, accountName);
            createContainer.CallCreateContainer();
        }

       

    }
}
