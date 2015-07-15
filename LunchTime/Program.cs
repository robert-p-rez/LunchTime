﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchTime
{
    class Program
    {
        private static readonly int MegaBytesCounter = 4;

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
                string filePath = ConstructFilePath(args);
                Patcher.Patch(filePath);
            }
        }

        private static string ConstructFilePath(string[] args)
        {
            string filePath = "";
            foreach (string word in args)
            {
                filePath += word + " ";
            }
            filePath = filePath.Substring(0, filePath.Length - 1);
            return filePath;
        }

        private static void RunApp()
        {
            MakeBurger();

            SlackNotifier.SendNotification();

            try
            {
                OutputMenu();
            }
            catch (MenuNotFoundException)
            {
                HandleMenuNotFoundException();
            }
            catch (NotServingException)
            {
                Console.WriteLine("Megabytes is not open today.");
            }
            catch (OldMenuException)
            {
                Console.WriteLine("\n\nThe menu has not been updated for this current week,\n rumor has it the front desk has the menu.");
            }

            TerminateLunchTime();
        }

        private static void OutputMenu()
        {
            WordDocumentParser parser = new WordDocumentParser();
            string link = IntergraphFileCreator.CurrentWeekFile();
            if (string.IsNullOrWhiteSpace(link))
            {
                if (IntergraphFileCreator.LastWeekFileStillExists())
                {
                    throw new OldMenuException();
                }
                throw new MenuNotFoundException();
            }
            parser.ReadFile(link);

            List<String> menu = parser.SmartGetDaysMenu();
            Console.WriteLine("\n\n");
            foreach (string line in menu)
            {
                Console.WriteLine(line.Trim());
                if (line.EndsWith("Y") || line.EndsWith("9"))
                {
                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// Run at the end of the application. Outputs the version number in '.' characters.
        /// </summary>
        private static void TerminateLunchTime()
        {
            Console.Write("Press enter to terminate");
            int i = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major;
            for (; i > 0; i--)
            {
                Console.Write(".");
            }
            Console.ReadKey();
        }

        private static void HandleMenuNotFoundException()
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday && DateTime.Now.Hour > 14)
            {
                Console.WriteLine("\n\nMegabytes is probably closed for the week, try again Monday.");
            }
            else
            {
                Console.WriteLine("\n\nThere was an error retrieving the menu.");
                Console.WriteLine("Megabytes has changed their website and broken this app {0} times.", MegaBytesCounter);
            }
        }

        private static void MakeBurger()
        {
            Console.WriteLine("       __________________");
            Console.WriteLine("   _.-\", ,' .'. ,  `. .  \"-._");
            Console.WriteLine(" .'. `    .    ,  `  .  ' '  `.");
            Console.WriteLine(".`____________________________'.");
            Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
            Console.WriteLine("`\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"'");
            Console.WriteLine(" `____________________________'");
            Console.WriteLine("\n\nDOWNLOADING MENU... PLZ W8 M8");
        }


    }
}
