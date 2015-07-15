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
        private static readonly string NetworkLocation = @"\\perez\shared\Lunchtime\LunchTime.exe";


        /// <summary>
        /// Starts the update process, should only be used if there is a new version on the network.
        /// </summary>
        internal static void Update()
        {
            string filePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Process patchProcess = new Process();
            patchProcess.StartInfo.FileName = NetworkLocation;
            patchProcess.StartInfo.Arguments = filePath;
            patchProcess.StartInfo.UseShellExecute = false;
            patchProcess.Start();
            Process updatedProcess = new Process();
            updatedProcess.StartInfo.FileName = NetworkLocation;
            updatedProcess.StartInfo.UseShellExecute = true;
            updatedProcess.Start();
        }

        /// <summary>
        /// Returns true if there is a new version of LunchTime on the network.
        /// </summary>
        internal static bool UpdateExists()
        {
            try
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo thisVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
                FileVersionInfo remoteVersion = FileVersionInfo.GetVersionInfo(NetworkLocation);
                if (remoteVersion.FileMajorPart > thisVersion.FileMajorPart)
                {
                    return true;
                }
            }
            catch { }

            return false;
        }

        /// <summary>
        /// Replaces the old version of LunchTime with the new version from the network location.
        /// </summary>
        internal static void Patch(string filePath)
        {
            System.Threading.Thread.Sleep(500);
            File.Delete(filePath);
            File.Copy(NetworkLocation, filePath);
        }

        /// <summary>
        /// Returns the version number of the executing assembly from the AssemblyInfo
        /// </summary>
        internal static int VersionNumber()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo thisVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
            return thisVersion.FileMajorPart;
        }

    }
}
