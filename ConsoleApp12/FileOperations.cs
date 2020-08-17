using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MusicTimer
{
    /// <summary>
    /// GetPlaylistsFromAllPaths does:
    /// 1. get all paths to playlists - in case they are saved on different locations
    /// 2. get folders and files - in case file is vlc playlist 
    /// 3. if file "browser.txt" exists those urls are listed as well
    /// </summary>
    public class FileOperations
    {
        public void WriteToPlaylist(List<string> playlist, string path)
        {
            using var file = File.CreateText(path);
            foreach (var p in playlist)
            {
                file.WriteLine(p);
            }
        }
        public List<string> GetPlaylistsFromAllPaths(string paths)
        {
            List<string> playlistPaths = GetPlaylistPaths(paths);
            List<string> all = new List<string>();

            foreach (var p in playlistPaths)
            {
                var songs = GetFolderAndFilesFromPath(p);
                all.AddRange(songs);
            }
            return all;
        }
        private List<string> GetPlaylistPaths(string paths)
        {
            return File.ReadAllLines(paths).ToList();
        }
        private List<string> GetFolderAndFilesFromPath(string path)
        {
            List<string> allPlaylists = new List<string>();

            try
            {
                var folders = Directory.GetDirectories(path);
                var files = Directory.GetFiles(path);

                foreach (string f in folders)
                {
                    string fileName = f.Substring(path.Length + 1);
                    allPlaylists.Add(f);
                }
                foreach (string f in files)
                {
                    string fileName = f.Substring(path.Length + 1);
                    allPlaylists.Add(f);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            var urlFile = path + "\\browser.txt";
            if (File.Exists(urlFile))
            {
                var urlLists = GetPlaylistsFromBrowser(urlFile);

                foreach (var u in urlLists)
                {
                    allPlaylists.Add(u);
                }
            }

            return allPlaylists;
        }
        private List<string> GetPlaylistsFromBrowser(string urlFile)
        {
            return File.ReadAllLines(urlFile).ToList();

        }

        // config operations
        public List<string> GetConfig(string path)
        {
            return File.ReadAllLines(path).ToList();
        }
        public void WriteToConfig(List<string> playlist, string path)
        {
            using var file = File.CreateText(path);
            foreach (var p in playlist)
            {
                file.WriteLine(p);
            }
        }
    }





}
