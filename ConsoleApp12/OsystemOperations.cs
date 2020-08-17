using MusicTimer.OperationSystems;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MusicTimer
{
    public class OsystemOperations : IOperationSystem
    {
        private static readonly IOperationSystem osystem = GetOs();

        public void MediaPlayerOn(string path)
        {
            osystem.MediaPlayerOn(path);
        }
        public void ScreenOff()
        {
            osystem.ScreenOff();
        }
        public void MediaPlayerOff()
        {
            osystem.MediaPlayerOff();
        }
        public int CheckBattery()
        {
            throw new NotImplementedException();
        }
        public void ConnectBluetooth()
        {
            throw new NotImplementedException();
        }
        public void ScreenOn()
        {
            osystem.ScreenOn();
        }
        public void Shutdown()
        {
            osystem.Shutdown();
        }
        public void CancelShutdown()
        {
            throw new NotImplementedException();
        }
        public void WarningAcoustic()
        {
            osystem.WarningAcoustic();
        }

        private static IOperationSystem GetOs()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new WindowsOs();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return null;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return null;
            }
            return null;
        }
    }
}
