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
            listContainer listContainer = new listContainer(accountName, accountKey,"mycontainer");
            listContainer.HttpListContainer();

            //CreateContainer createContainer = new CreateContainer(accountName, accountKey,"jambor2");
            //createContainer.CallCreateContainer();

            /*****************create container**************/
            //CreateQueue createContainer = new CreateQueue(accountName, accountKey, "jambor2");
            //createContainer.CallCreateQueue();

            /*****************put message**************/
            PutMessageToQueue putMessage = new PutMessageToQueue(accountName, accountKey, "jambor2");
            putMessage.SendMessage();

        }

       

    }
}
