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
            if (args.Length == 0)
            {
                if (Patcher.UpdateExists())
                {
                    Patcher.Update();
                }
                else
                {
                    RunApp();
                }
            }
            else
            {
                string filePath = "";
                foreach (string word in args)
                {
                    filePath += word + " ";
                }
                filePath = filePath.Substring(0, filePath.Length - 1);
                Patcher.Patch(filePath);
            }
        }

        private static void RunApp()
        {
            SlackNotifier.SendNotification();

            int megaBytesCounter = 4;

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
                if (string.IsNullOrWhiteSpace(link))
                {
                    throw new MenuNotFoundException();
                }
                parser.ReadFile(link);

                List<String> menu = parser.SmartGetDaysMenu();
                Console.Clear();
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
                Console.WriteLine("Megabytes has changed their website and broken this app {0} times.", megaBytesCounter);
            }
            catch (NotServingException)
            {
                Console.Clear();
                Console.WriteLine("Megabytes is not open today.");
            }
            Console.WriteLine();
            Console.Write("Press enter to terminate.");
            int i = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major;
            for (; i > 0; i--)
            {
                Console.Write(".");
            }
            Console.ReadKey();
        }


    }
}
