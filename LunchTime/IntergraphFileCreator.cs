using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LunchTime
{
    class IntergraphFileCreator
    {

        //Should return filepath like "http://305.intergraph.com/wp-content/uploads/2015/05/Megabytes-5-24-15.docx" based on the current date
        public static string CurrentWeekFile()
        {
            return MakeDate();
        }

        private static string MakeDate()
        {
           // string output = 
            DateTime today = DateTime.Now;
            switch (today.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    throw new NotServingException();
                case DayOfWeek.Friday:
                    today = today.AddDays(+2);
                    break;
                case DayOfWeek.Thursday:
                    today = today.AddDays(+3);
                    break;
                case DayOfWeek.Wednesday:
                    today = today.AddDays(+4);
                    break;
                case DayOfWeek.Tuesday:
                    today = today.AddDays(+5);
                    break;
                case DayOfWeek.Monday:
                    today = today.AddDays(+6);
                    break;
                default:
                    break;
            }
            return "http://305.intergraph.com/wp-content/uploads/2015/06/Megabytes-" + today.Month.ToString() + "-" + today.Day.ToString() + "-" + today.Year.ToString().Remove(0, 2)+ ".docx";;
        }

        private static string ParseHTMLLink()
        {
            WebRequest req = HttpWebRequest.Create("http://305.intergraph.com/?page_id=796");
            req.Method = "GET";
            string html;
            try
            {
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    html = reader.ReadToEnd();
                }
            }
            catch {
                return MakeDate();
            }
            foreach (string item in html.Split('\"'))
            {
                if (item.Contains("Megabytes") && item.Contains("http://305.intergraph.com/wp-content"))
                    return item;
            }
            return "";
        }
    }
}
