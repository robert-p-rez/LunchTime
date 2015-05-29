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

        private void ParseWithWord(string filePath)
        {
            StreamWriter writer = new StreamWriter(Path.GetTempPath() + @"LunchTimeMenu.txt");
            writer.WriteLine(filePath);
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document docs = null;
            //word.Visible = false;
            object miss = System.Reflection.Missing.Value;
            object path = filePath;
            object readOnly = true;
            try
            {
                docs = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);
                for (int i = 0; i < docs.Paragraphs.Count; i++)
                {
                    string item= docs.Paragraphs[i + 1].Range.Text.ToString();
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
                if (docs != null)
                {
                    docs.Close();
                }
                word.Quit();
            }
        }

        private bool DoesCurrentCacheExist(string filepath)
        {
            bool exists = false;
            if (!File.Exists(Path.GetTempPath() + "LunchTimeMenu.txt"))
            {
                return exists;
            }

            StreamReader fileReader = new StreamReader(Path.GetTempPath() + "LunchTimeMenu.txt");
            if (fileReader.ReadLine() == filepath)
            {
                exists = !exists;
            }
            fileReader.Close();
            return exists;
        }

        public List<String> GetAllFileText()
        {
            return fileText;
        }

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
