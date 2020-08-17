using System;
using System.Collections.Generic;
using System.Text;

namespace MusicTimer.OperationSystems
{
    interface IOperationSystem
    {
        int CheckBattery();
        void ConnectBluetooth();
        void MediaPlayerOn(string path);
        void MediaPlayerOff();
        void ScreenOn();
        void ScreenOff();
        void Shutdown();
        void CancelShutdown();
        void WarningAcoustic();
    }
}
