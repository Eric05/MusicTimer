using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MusicTimer.OperationSystems
{
    //TODO
    // get mediaplayer and path by textfile

    /// <summary>
    /// string strCmdText = "taskkill / im PhotoScreensaver.scr /f";
    /// Process.Start("CMD.exe", strCmdText);
    /// </summary>
    public class WindowsOs : IOperationSystem
    {
        public void CancelShutdown()
        {
            Process.Start("shutdown", "/F");
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
            Process.Start("taskkill", "/F /IM vlc.exe");        
        }

        public void MediaPlayerOn(string path)
        {
            if (path.StartsWith("http") || path.StartsWith("www."))
            {
                Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    Arguments = "/c start firefox " + "\"" + path + "\"",
                    CreateNoWindow = true,
                    FileName = "CMD.exe"
                });
            }
            else
            {
                string str = @"C:\Program Files\VideoLAN\VLC\vlc.exe";
                string res = "\"" + str + "\"" + " " + "\"" + path + "\"";
                Process.Start(res);
            }
            //Process.Start( @"C:\Program Files\VideoLAN\VLC\vlc.exe" + "\"" + path + "\"");
        }

        public void ScreenOff()
        {
            string strCmdText = "/C %systemroot%/system32/scrnsave.scr /s";
            Process.Start("CMD.exe", strCmdText);         
        }

        public void ScreenOn()
        {
            Process.Start("taskkill", "/F /IM scrnsave.scr");
        }

        public void Shutdown()
        {
            Process.Start("shutdown", "/S /F");       
        }

        public void WarningAcoustic()
        {
            Console.Beep();
        }
    }
}
