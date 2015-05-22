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
            string menu = WordDocumentParser.ReadFile(IntergraphFileCreator.CurrentWeekFile());
            //MessageBox.Show(todaysMenuString);
        }

        
    }
}
