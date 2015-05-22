using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchTime
{
    class IntergraphFileCreator
    {

        //Should return filepath like "http://305.intergraph.com/wp-content/uploads/2015/05/Megabytes-5-24-15.docx" based on the current date
        public static string CurrentWeekFile()
        {
            return "http://305.intergraph.com/wp-content/uploads/2015/05/Megabytes-" + MakeDate() + ".docx";
        }

        private static string MakeDate()
        {
           // string output = 
            DateTime today = DateTime.Now;

            switch (today.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    throw new NotImplementedException();
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
                default:
                    break;
            }
            return today.Month.ToString() + "-" + today.Day.ToString() + "-" + today.Year.ToString().Remove(0,2);
        }
    }
}
