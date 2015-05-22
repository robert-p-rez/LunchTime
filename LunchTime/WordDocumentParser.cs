using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchTime
{
    public class WordDocumentParser
    {

        //code copied from http://mantascode.com/c-how-to-parse-the-text-content-from-microsoft-word-document/
        public static string ReadFile(string filePath)
        {
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            //word.Visible = false;
            object miss = System.Reflection.Missing.Value;
            object path = filePath;
            object readOnly = true;
            Console.WriteLine("test1");
            Microsoft.Office.Interop.Word.Document docs = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);
            Console.WriteLine("test2");
            string totaltext = "";
            for (int i = 0; i < docs.Paragraphs.Count; i++)
            {
                   totaltext += " \r\n "+ docs.Paragraphs[i+1].Range.Text.ToString();
                   Console.Write(docs.Paragraphs[i + 1].Range.Text.ToString());
            }
            docs.Close();
            word.Quit();
            return totaltext;
        }
    }
}
