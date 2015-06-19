using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;

namespace LunchTime
{
    internal class SlackNotifier
    {
        internal static void SendNotification()
        {
            try
            {
                SendTextToSlack(GetCurrentUser());
            }
            catch
            {
                try
                {
                    SendTextToSlack("Someone used lunchtime!");
                }
                catch
                {
                    //really bad failure :(
                }
            }
        }

        private static string GetCurrentUser()
        {
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name.Substring(8);
        }

        private static void SendTextToSlack(string text)
        {
            string message = text + " is hungry!";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://slack.com/api/chat.postMessage?token=test&channel=%23lunchtime&text=" + message + "&username=lunchbox&icon_emoji=%3Ahamburger%3A");
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";


            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

        }

    }
}
