using MusicTimer.OperationSystems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MusicTimer
{
    class Program
    {
        //TODO configFile

        public static readonly string pathToPlaylists = "..\\Playlists.txt";
        public static readonly string pathToConfig = "..\\Config.txt";
        static void Main()
        {
            FileOperations fop = new FileOperations();
            OsystemOperations op = new OsystemOperations();

            string pathToPlaylist = fop.GetConfig(pathToConfig)[0];
            int shutdownTime = fop.GetConfig(pathToConfig)[1].ToMilliseconds();

            PrintHeader();
            PrintColors();
            PrintInitialSettings();
            PrintColors();

            var task = Task.Factory.StartNew(() => op.MediaPlayerOn(pathToPlaylist));
            StartRoutine(shutdownTime);
        }
        private static void PrintHeader()
        {
            ChangeColors(ConsoleColor.Green, ConsoleColor.Black);
            Console.WriteLine();
            Console.WriteLine("\t     YOUR MUSIC - YOUR TIMER       ");
            Console.ForegroundColor = ConsoleColor.White;
        }
        private static void PrintColors()
        {
            Console.WriteLine();

            for (int i = 0; i < 8; i++)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Thread.Sleep(100);
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write("_____");
                Thread.Sleep(100);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("_____");
            }

            Console.WriteLine();
            ChangeColors(ConsoleColor.White, ConsoleColor.Black);
        }
        private static void PrintInitialSettings()
        {
            FileOperations fop = new FileOperations();

            var config = new List<string>();

            try
            {
                config = fop.GetConfig(pathToConfig);
            }
            catch
            {
                Console.WriteLine("An Error occured: Please set correct path");
                Console.ReadLine();
            }

            var playlist = fop.GetConfig(pathToConfig)[0].SplitPath();
            string cleanName = Path.GetFileNameWithoutExtension(playlist[playlist.Length - 1]);
            string res = $" Starting Playlist:\t{cleanName}\n Timer running for:\t{config[1]} mins";

            Console.WriteLine(res);
        }
        private static void PrintMenu(string increaseShutdown)
        {
            Console.WriteLine(
                $"\tPress (ENTER) to increase Timer for {increaseShutdown} mins\n " +
                $"\tWrite any time you want Timer to increase for\n " +
                $"\tPress (s) to change Settings\n " +
                $"\tPress (c) to cancel app\n " +
                $"\tPress (t) to cancel timer\n "
                               );
        }
        private static void HandleMenuInput()
        {
            OsystemOperations op = new OsystemOperations();
            FileOperations fop = new FileOperations();
            string increaseTimer = fop.GetConfig(pathToConfig)[3];

            string inp = Console.ReadLine();

            switch (inp)
            {
                case "t":
                    TimerOperations.Cancel();
                    break;

                case "c":
                    op.MediaPlayerOff();
                    TimerOperations.Cancel();
                    Environment.Exit(0);
                    break;

                case "s":
                    var newSettings = ChangeSettings();

                    try
                    {
                        op.MediaPlayerOff();
                    }
                    catch
                    {
                        
                    }
                    Thread.Sleep(1000);
                    Console.Clear();
                    PrintColors();                    
                    Console.WriteLine();
                    var task = Task.Factory.StartNew(() => op.MediaPlayerOn(newSettings.Item1));

                    TimerOperations.Cancel();
                    StartRoutine(newSettings.Item2.ToMilliseconds());
                    break;

                case "":
                    Console.WriteLine();
                    Console.WriteLine(" Timer increased for " + increaseTimer + " mins");
                    Console.WriteLine();
                    TimerOperations.Cancel();
                    StartRoutine(increaseTimer.ToMilliseconds());
                    break;

                default:
                    int num;
                    if (int.TryParse(inp, out num))
                    {
                        Console.WriteLine();
                        Console.WriteLine(" Timer increased for " + num + " mins");
                        Console.WriteLine();
                        TimerOperations.Cancel();
                        StartRoutine(inp.ToMilliseconds());
                        break;
                    }
                    break;
            }

            Console.ReadLine();
        }
        private static (string, string) ChangeSettings()
        {
            Console.Clear();
            PrintColors();
            ChangeColors(ConsoleColor.Yellow, ConsoleColor.Black);

            FileOperations fop = new FileOperations();
            var list = fop.GetPlaylistsFromAllPaths(pathToPlaylists);
            var conf = fop.GetConfig(pathToConfig);

            Console.WriteLine();
            ChangeColors(ConsoleColor.Green, ConsoleColor.Black);
            Console.WriteLine("AVAILABLE PLAYLISTS: ");
            ChangeColors(ConsoleColor.White, ConsoleColor.Black);
            Console.WriteLine();
            PrintAllPlaylists(list);

            Console.WriteLine();
            Console.WriteLine("Enter new playlist by number: ");
            var play = Console.ReadLine();

            Console.WriteLine("Enter new Timer: ");
            var timer = Console.ReadLine();

            Console.WriteLine("Press (ENTER) to save settings permanently or...");
            Console.WriteLine("      (t) to use those settings temporarly");
            var save = Console.ReadLine();

            string newPlay = !IsValidPlaylist(play, list.Count) ? fop.GetConfig(pathToConfig)[0] : list[Convert.ToInt32(play) - 1];
            string newTime = !IsValidTimer(timer) ? fop.GetConfig(pathToConfig)[1] : timer;

            if (save == "")
            {
                conf[0] = newPlay;
                conf[1] = newTime;

                fop.WriteToConfig(conf, pathToConfig);
            }
            ChangeColors(ConsoleColor.White, ConsoleColor.Black);

            return (newPlay, newTime);
        }
        private static void StartRoutine(int shutTime)
        {
            FileOperations fop = new FileOperations();
            OsystemOperations op = new OsystemOperations();

            string increaseTimer = fop.GetConfig(pathToConfig)[3];
            int warnTime = fop.GetConfig(pathToConfig)[2].ToMilliseconds();
            int waitBeforeScreenOff = Convert.ToInt32(fop.GetConfig(pathToConfig)[4]) * 1000;

            PrintMenu(increaseTimer);

            var task = Task.Factory.StartNew(() => TimerOperations.SetTimerAndWarning(OnTimerCompleted, OnWarningCompleted, shutTime, warnTime));
            var task1 = Task.Factory.StartNew(() => { Thread.Sleep(waitBeforeScreenOff); op.ScreenOff(); });

            HandleMenuInput();
        }
        private static void PrintAllPlaylists(List<string> list)
        {
            int counter = 0;

            foreach (var l in list)
            {
                counter++;
                var res = l.Split("\\");
                var x = Path.GetFileNameWithoutExtension(res[^1]);
                Console.WriteLine("\t" + counter + ":\t" + x);
            }
        }
        private static void ChangeColors(ConsoleColor fore, ConsoleColor back)
        {
            Console.ForegroundColor = fore;
            Console.BackgroundColor = back;
        }
        public static void WarnBeforeShut()
        {
            FileOperations fop = new FileOperations();
            OsystemOperations op = new OsystemOperations();

            op.ScreenOn();
            op.WarningAcoustic();
            Thread.Sleep(1500);
            op.WarningAcoustic();
            Console.Clear();

            PrintColors();
            Console.WriteLine();
            ChangeColors(ConsoleColor.White, ConsoleColor.Red);
            Console.WriteLine("WARNING ! Timer ends in " + fop.GetConfig(pathToConfig)[2] + " min");
            Console.WriteLine();
            ChangeColors(ConsoleColor.White, ConsoleColor.Black);

            PrintMenu(fop.GetConfig(pathToConfig)[3]);
            HandleMenuInput();
        }

        /// <summary>
        /// Actions when Timer completes
        /// </summary>
        private static void OnTimerCompleted()
        {
            OsystemOperations op = new OsystemOperations();
            op.Shutdown();
        }
        private static void OnWarningCompleted()
        {
            WarnBeforeShut();
        }

        /// <summary>
        /// Helper Methods for validation
        /// </summary>     
        private static bool IsValidPlaylist(string playlist, int max)
        {
            _ = int.TryParse(playlist, out int playlistNumber);

            if (playlistNumber > 0 && playlistNumber < max)
            {
                return true;
            }
            return false;
        }
        private static bool IsValidTimer(string timer)
        {
            _ = int.TryParse(timer, out int num);

            if (num > 0)
            {
                return true;
            }
            return false;
        }
    }
    /// <summary>
    /// Extension Method1: converts a string of minutes to int of milliseconds
    /// Extension Method2: splits path for windows and linux
    /// </summary>
    public static class HelperMethods
    {
        public static int ToMilliseconds(this string str)
        {
            int res = Convert.ToInt32(str) * 60000;
            return res;
        }

        public static string[] SplitPath(this string str)
        {
            string[] res = str.Split(new char[] { '\\', '/' });
            return res;
        }
    }

}
