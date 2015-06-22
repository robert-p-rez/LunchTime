using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchTime
{
    internal class Patcher
    {

        internal static void Update()
        {
            string filePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Process test = new Process();
            test.StartInfo.FileName = @"\\perez\shared\Lunchtime\LunchTime.exe";
            test.StartInfo.Arguments = filePath;
            test.StartInfo.UseShellExecute = false;
            test.Start();
        }

        internal static bool UpdateExists()
        {
            try
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo thisVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
                FileVersionInfo remoteVersion = FileVersionInfo.GetVersionInfo(@"\\perez\shared\Lunchtime\LunchTime.exe");
                if (remoteVersion.FileMajorPart > thisVersion.FileMajorPart)
                {
                    return true;
                }
            }
            catch { }
            
            return false;
        }

        internal static void Patch(string filePath)
        {
            System.Threading.Thread.Sleep(500);
            File.Delete(filePath);
            File.Copy(@"\\perez\shared\Lunchtime\LunchTime.exe", filePath);
        }

    }
}
