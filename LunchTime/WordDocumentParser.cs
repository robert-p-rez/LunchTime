using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchTime
{
    public class WordDocumentParser
    {
        private List<string> fileText;

        public WordDocumentParser()
        {
            fileText = new List<String>();
        }

        //code copied from http://mantascode.com/c-how-to-parse-the-text-content-from-microsoft-word-document/
        public void ReadFile(string filePath)
        {
            if (DoesCurrentCacheExist(filePath))
            {
                ParseCache();
            }
            else
            {
                ParseWithWord(filePath);
            }
        }

        /// <summary>
        /// Parses the users temp folder for the cached menu.
        /// </summary>
        private void ParseCache()
        {
            StreamReader reader = new StreamReader(Path.GetTempPath() + "LunchTimeMenu.txt");
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                fileText.Add(reader.ReadLine());
            }
            reader.Close();
        }

        /// <summary>
        /// If no cache was found then we have to parse the document with Word.
        /// </summary>
        private void ParseWithWord(string filePath)
        {
            StreamWriter writer = new StreamWriter(Path.GetTempPath() + "LunchTimeMenu.txt");
            writer.WriteLine(filePath);
            _Application word = new Application();
            _Document doc = null;
            object miss = System.Reflection.Missing.Value;
            object path = filePath;
            object readOnly = true;
            try
            {
                doc = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);
                for (int i = 0; i < doc.Paragraphs.Count; i++)
                {
                    string item= doc.Paragraphs[i + 1].Range.Text.ToString();
                    fileText.Add(item);
                    writer.WriteLine(item);
                }
            }
            catch
            {
                throw new MenuNotFoundException();
            }
            finally
            {
                writer.Flush();
                writer.Close();
                if (doc != null)
                {
                    doc.Close(Type.Missing,Type.Missing,Type.Missing);
                }
                word.Quit();
            }
        }

        /// <summary>
        /// Returns true if the user has the correct cached menu in their temp folder.
        /// </summary>
        private bool DoesCurrentCacheExist(string filepath)
        {
            bool exists = false;

            if (!File.Exists(Path.GetTempPath() + "LunchTimeMenu.txt"))
            {
                return exists;
            }

            using (StreamReader fileReader = new StreamReader(Path.GetTempPath() + "LunchTimeMenu.txt"))
            {
                return (fileReader.ReadLine() == filepath) ? true : false;
            }
        }

        public List<String> GetAllFileText()
        {
            return fileText;
        }

        /// <summary>
        /// Returns the menu for a specific day.
        /// </summary>
        public List<string> GetDaysMenu(DayOfWeek theDate)
        {
            if (theDate == DayOfWeek.Sunday || theDate == DayOfWeek.Saturday)
            {
                throw new NotServingException();
            }
            bool startedReading = false;
            List<string> lines = new List<string>();

            foreach (string line in fileText)
            {
                if (line.Contains(theDate.ToString().ToUpper()))
                {
                    startedReading = true;
                }
                if (startedReading && LineContainsAnotherDay(line, theDate))
                {
                    startedReading = false;
                }
                if (startedReading && !string.IsNullOrWhiteSpace(line) && line != "\r\a" && line != "\a")
                {
                    lines.Add(line.Replace("\r\a", ""));
                }
            }

            return lines;
        }

        /// <summary>
        /// Returns the menu for today if megabytes is open or is yet to open, the menu for tomorrow is returned otherwise.
        /// </summary>
        public List<string> SmartGetDaysMenu()
        {
            int hour = System.DateTime.Now.Hour;
            int minute = System.DateTime.Now.Minute;

            List<string> menu;
            if (hour > 14 || (hour == 14 && minute >= 35))
            {
                menu = GetDaysMenu(DateTime.Now.AddHours(14).DayOfWeek);
            }
            else
            {
                menu = GetDaysMenu(DateTime.Now.DayOfWeek);
                if (hour == 13 && minute > 1)
                {
                    StripClosedStations(ref menu);
                }
            }

            return menu;
        }

        /// <summary>
        /// This should remove the closed food stations because expo and deli close earlier than the others.
        /// </summary>
        private void StripClosedStations(ref List<String> menu)
        {
            for (int i = 0; i < menu.Count; i++)
            {
                if (menu[i].Contains("Expo Station") || menu[i].Contains("Deli"))
                {
                    for (int j = 0; j < 3; j++ )
                        menu.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Returns true if the string line contains a day of the week.
        /// Used to determine the end and beginning of daily menus.
        /// </summary>
        private bool LineContainsAnotherDay(string line, DayOfWeek dayToNotCheckFor)
        {
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                if (line.Contains(day.ToString().ToUpper()) && dayToNotCheckFor != day)
                {
                    return true;
                }
            }
            return false;
        }


    }
}
