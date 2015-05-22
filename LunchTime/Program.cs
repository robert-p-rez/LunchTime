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
            WordDocumentParser parser = new WordDocumentParser();
            parser.ReadFile(IntergraphFileCreator.CurrentWeekFile());
            foreach (string line in parser.GetDaysMenu(DateTime.Now.DayOfWeek))
            {
                Console.WriteLine(line.Trim());
                if (line.EndsWith("Y") || line.EndsWith("9"))
                {
                    Console.WriteLine();
                }
            }
            Console.ReadKey();
        }

        
    }
}
