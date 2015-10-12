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

        /// <summary>
        /// Returns the location of the menu for the current week if it exists.
        /// </summary>
        public static string CurrentWeekFile()
        {
            return ParseHTMLLink(DateTime.Now);
        }

        /// <summary>
        /// Test for if the old menu is still on the website.
        /// </summary>
        internal static bool LastWeekFileStillExists()
        {
            if (string.IsNullOrWhiteSpace(ParseHTMLLink(DateTime.Now.AddDays(-7))))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Makes expected file location based on previous experience.
        /// In the form: http://305.intergraph.com/wp-content/uploads/2015/07/Megabytes-7-19-15.docx
        /// </summary>
        /// <param name="dateToUse">The date to use.</param>
        /// <returns></returns>
        /// <exception cref="NotServingException">Thrown if megabytes is not open today.</exception>
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
            return "http://305.intergraph.com/wp-content/uploads/"+dateToUse.Year.ToString()+"/0"+(dateToUse.Month.ToString().Length == 1 ? "0":"") + dateToUse.Month.ToString()  +"/Megabytes-" + dateToUse.Month.ToString() + "-" + dateToUse.Day.ToString() + "-" + dateToUse.Year.ToString().Remove(0, 2) + ".pdf";
        }

        /// <summary>
        /// Parses the wedsite for the menu location, should match the filename created by MakeDate()
        /// </summary>
        /// <param name="dateToUse">The date to use.</param>
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

        /// <summary>
        /// Returns the expected filename for the megabytes menu.
        /// </summary>
        private static string FileName(DateTime dateToUse)
        {
            return Path.GetFileName(MakeDate(dateToUse)).Substring(10);
        }
    }
}
