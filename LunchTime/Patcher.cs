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
            Process patchProcess = new Process();
            patchProcess.StartInfo.FileName = @"\\perez\shared\Lunchtime\LunchTime.exe";
            patchProcess.StartInfo.Arguments = filePath;
            patchProcess.StartInfo.UseShellExecute = false;
            patchProcess.Start();
            Process updatedProcess = new Process();
            updatedProcess.StartInfo.FileName = @"\\perez\shared\Lunchtime\LunchTime.exe";
            updatedProcess.StartInfo.UseShellExecute = true;
            updatedProcess.Start();
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
