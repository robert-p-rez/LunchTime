using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    if (!GUIExists())
                    {
                    RunApp();
                    }
                    else
                    {
                        SlackNotifier.SendNotification("G");
                    }
                }
            }
            else
            {
                string filePath = ConstructFilePath(args);
                Patcher.Patch(filePath);
            }
        }

        private static bool GUIExists()
        {
            string NetworkLocation = @"\\perez\shared\Lunchtime\GUI\LunchTimeGui.exe";
            if (File.Exists(NetworkLocation))
            {
                Process GUIProcess = new Process();
                GUIProcess.StartInfo.FileName = NetworkLocation;
                GUIProcess.StartInfo.UseShellExecute = false;
                GUIProcess.Start();
                return true;
            }
            return false;
        }

        private static void RunApp()
        {
            MakeBurger();

          //  SlackNotifier.SendNotification();

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

        /// <summary>
        /// The actual workflow to parse the menu.
        /// </summary>
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

        /// <summary>
        /// Exception handling for after Megabytes closes and before Monday. Or if the website changes.
        /// </summary>
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

        /// <summary>
        /// ASCII burger.
        /// </summary>
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

        /// <summary>
        /// Constructs one string from the filepath passed in by command line arguments. (The location of the exe a user is running)
        /// </summary>
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
    }
}
