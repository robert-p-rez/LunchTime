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
                SendTextToSlack("Someone used lunchtime!");
            }
        }

        private static string GetCurrentUser()
        {
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name.Substring(8);
        }

        private static void SendTextToSlack(string text)
        {
            var connectionInfo = new SlackSettings();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://hooks.slack.com/services/" + connectionInfo.URL);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"text\":\"" + text + " was hungry.\",\"username\": \"lunchbox\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }

    }
}
