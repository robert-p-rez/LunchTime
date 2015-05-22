using System;
using System.Collections.Generic;
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
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            //word.Visible = false;
            object miss = System.Reflection.Missing.Value;
            object path = filePath;
            object readOnly = true;
            Microsoft.Office.Interop.Word.Document docs = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);
            for (int i = 0; i < docs.Paragraphs.Count; i++)
            {
                fileText.Add(docs.Paragraphs[i + 1].Range.Text.ToString());
            }
            docs.Close();
            word.Quit();
        }

        public List<String> GetAllFileText()
        {
            return fileText;
        }

        public List<string> GetDaysMenu(DayOfWeek theDate)
        {
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
                if (startedReading && !string.IsNullOrWhiteSpace(line) && line != "\r\a")
                {
                    lines.Add(line.Replace("\r\a",""));
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
