using System;
using System.Diagnostics;
using System.Linq;
using Anotar.CommonLogging;
using NullGuard;

namespace AzureMagic.Tools
{
    /// <summary>
    ///     Original source: http://stackoverflow.com/a/18094666/22941 by http://stackoverflow.com/users/607701/dpeden
    /// </summary>
    public static class AzureStorageEmulatorManager
    {
        private const string ProcessName = "DSServiceLDB";

        private static readonly object ThisLock = new Object(); 
        private static bool StartingProcess;

        private static readonly ProcessStartInfo ProcessStartInfo = new ProcessStartInfo
        {
            FileName = @"C:\Program Files\Microsoft SDKs\Windows Azure\Emulator\csrun.exe",
            Arguments = "/devstore",
            WindowStyle = ProcessWindowStyle.Hidden
        };

        [return: AllowNull]
        public static Process GetProcess()
        {
            lock (ThisLock)
            {
                return Process.GetProcessesByName(ProcessName).FirstOrDefault();
            }
        }

        public static bool IsProcessStarted()
        {
            lock (ThisLock)
            {
                return GetProcess() != null;
            }
        }

        public static void StartProcess()
        {
            lock (ThisLock)
            {
                if (IsProcessStarted() || StartingProcess)
                {
                    return;
                }

                StartingProcess = true;

                using (var process = Process.Start(ProcessStartInfo))
                {
                    LogTo.Debug("Starting Storage Emulator...");
                    process.WaitForExit();
                    LogTo.Debug("Azure Storage Emulator has started.");
                }

                StartingProcess = false;
            }
        }

        public static void StopProcess()
        {
            lock (ThisLock)
            {
                var process = GetProcess();

                if (process != null)
                {
                    process.Kill();
                }
            }
        }
    }
}