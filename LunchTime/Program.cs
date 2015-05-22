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
            StreamWriter fileWriter = new StreamWriter(@"C:\temp\test.txt");
            WordDocumentParser parser = new WordDocumentParser();
            parser.ReadFile(IntergraphFileCreator.CurrentWeekFile());
            foreach (string line in parser.GetDaysMenu(DateTime.Now.DayOfWeek))
            {
                fileWriter.WriteLine(line);
                Console.WriteLine(line);
            }
            fileWriter.Close();
            Console.ReadLine();
            //MessageBox.Show(todaysMenuString);
        }

        
    }
}
