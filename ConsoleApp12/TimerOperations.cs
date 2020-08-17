using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusicTimer
{
    /// <summary>
    /// set timer for shutdown and warning
    /// when timer finish they exectute void action
    /// </summary>
    public class TimerOperations
    {
        public static Action StartShutdown;
        public static Action Warning;
        private static Timer t;
        private static Timer t1;
        public static void SetTimerAndWarning(Action actionShut, Action actionWarn, int time, int warnTimespan)
        {
            int warnTime = Math.Abs(time - warnTimespan);
            StartShutdown = actionShut;
            Warning = actionWarn;

            t = new Timer(InvokeShutdown, null, time, Timeout.Infinite);
            t1 = new Timer(InvokeWarning, null, warnTime, Timeout.Infinite);
        }
        private static void InvokeShutdown(object state)
        {
            StartShutdown();
        }
        private static void InvokeWarning(object state)
        {
            Warning();
        }
        public static void Cancel()
        {
            t.Change(Timeout.Infinite, Timeout.Infinite);
            t1.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
}
