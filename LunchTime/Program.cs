using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchTime
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("       __________________");
            Console.WriteLine("   _.-\", ,' .'. ,  `. .  \"-._");
            Console.WriteLine(" .'. `    .    ,  `  .  ' '  `.");
            Console.WriteLine(".`____________________________'.");
            Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
            Console.WriteLine("`\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"'");
            Console.WriteLine(" `____________________________'");
            Console.WriteLine("\n\nDOWNLOADING MENU... PLZ W8 M8");

            WordDocumentParser parser = new WordDocumentParser();
            try
            {
                string link = IntergraphFileCreator.CurrentWeekFile();
                parser.ReadFile(link);

                DateTime menuDay = DateTime.Now;
                if (menuDay.AddHours(10).Date != menuDay.Date)
                {
                    menuDay = menuDay.AddHours(10);
                }
                List<String> menu = parser.GetDaysMenu(menuDay.DayOfWeek);
                foreach (string line in menu)
                {
                    Console.WriteLine(line.Trim());
                    if (line.EndsWith("Y") || line.EndsWith("9"))
                    {
                        Console.WriteLine();
                    }
                }
            }
            catch (MenuNotFoundException)
            {
                Console.Clear();
                Console.WriteLine("There was an error retrieving the menu.");
            }
            catch (NotServingException)
            {
                Console.Clear();
                Console.WriteLine("Megabytes is not open today.");
            }
            Console.WriteLine();
            Console.WriteLine("Press enter to terminate...");
            Console.ReadKey();
        }


    }
}
