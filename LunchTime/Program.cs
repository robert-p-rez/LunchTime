using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchTime
{
    class Program
    {
        static void Main(string[] args)
        {
            string menu = WordDocumentParser.ReadFile("http://305.intergraph.com/wp-content/uploads/2015/05/Megabytes-5-24-15.docx");
            //MessageBox.Show(todaysMenuString);
        }

        
    }
}
