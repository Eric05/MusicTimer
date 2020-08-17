using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MusicTimer.OperationSystems
{
    //TODO
    public class LinuxOs : IOperationSystem
    {
        public void CancelShutdown()
        {
            throw new NotImplementedException();
        }

        public int CheckBattery()
        {
            throw new NotImplementedException();
        }

        public void ConnectBluetooth()
        {
            throw new NotImplementedException();
        }

        public void MediaPlayerOff()
        {
            throw new NotImplementedException();
        }

        public void MediaPlayerOn(string path)
        {
            throw new NotImplementedException();
        }

        public void ScreenOff()
        {
            throw new NotImplementedException();
        }

        public void ScreenOn()
        {
            throw new NotImplementedException();
        }

        public void Shutdown()
        {
            "shutdown".Bash();
        }

        public void WarningAcoustic()
        {
            throw new NotImplementedException();
        }
    }

    public static class BashHelper
    {
        public static void Bash(this string cmd)
        {
            var cleanArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{cleanArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();          
        }
    }
}
