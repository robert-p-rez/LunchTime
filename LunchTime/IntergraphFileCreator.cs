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
            return ParseHTMLLink(DateTime.Now);
        }

        internal static bool LastWeekFileStillExists()
        {
            if (string.IsNullOrWhiteSpace(ParseHTMLLink(DateTime.Now.AddDays(-7))))
            {
                return false;
            }
            return true;
        }

        private static string MakeDate(DateTime dateToUse)
        {
            switch (dateToUse.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    throw new NotServingException();
                case DayOfWeek.Friday:
                    dateToUse = dateToUse.AddDays(+2);
                    break;
                case DayOfWeek.Thursday:
                    dateToUse = dateToUse.AddDays(+3);
                    break;
                case DayOfWeek.Wednesday:
                    dateToUse = dateToUse.AddDays(+4);
                    break;
                case DayOfWeek.Tuesday:
                    dateToUse = dateToUse.AddDays(+5);
                    break;
                case DayOfWeek.Monday:
                    dateToUse = dateToUse.AddDays(+6);
                    break;
                default:
                    break;
            }
            return "http://305.intergraph.com/wp-content/uploads/2015/0"+dateToUse.Month.ToString()+"/Megabytes-" + dateToUse.Month.ToString() + "-" + dateToUse.Day.ToString() + "-" + dateToUse.Year.ToString().Remove(0, 2) + ".docx"; ;
        }

        private static string ParseHTMLLink(DateTime dateToUse)
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
            catch
            {
                return MakeDate(dateToUse);
            }

            foreach (string item in html.Split('\"'))
            {
                if (item.Contains("Megabytes") && item.Contains("http://305.intergraph.com/wp-content"))
                    if (item.EndsWith(FileName(dateToUse)))
                    {
                        return item;
                    }
            }
            return "";
        }

        private static string FileName(DateTime dateToUse)
        {
            return Path.GetFileName(MakeDate(dateToUse));
        }
    }
}
