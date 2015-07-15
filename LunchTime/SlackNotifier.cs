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
        /// <summary>
        /// Send an update about hunger to slack.
        /// </summary>
        internal static void SendNotification()
        {
            try
            {
                SendTextToSlack(GetCurrentUser()+ " is hungry! v"+ Patcher.VersionNumber());
            }
            catch
            {
                try
                {
                    SendTextToSlack("Someone used lunchtime! v" + Patcher.VersionNumber());
                }
                catch
                {
                    //really bad failure :(
                }
            }
        }

        /// <summary>
        /// Gets the current user's domain name.
        /// </summary>
        private static string GetCurrentUser()
        {
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name.Substring(8);
        }

        /// <summary>
        /// Actual HTTP request to update slack with queries.
        /// </summary>
        private static void SendTextToSlack(string text)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://slack.com/api/chat.postMessage?token=test&channel=%23lunchtime&text=" + text + "&username=lunchbox&icon_emoji=%3Ahamburger%3A");
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";


            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

        }

    }
}
