using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IllicitPullerPrivate.Classes
{
    class LogsLib
    {
        public static BindingList<LogsInfo> LogsInfoGrabbed = new BindingList<LogsInfo>();
        private static string FilePath = Application.ExecutablePath.Replace($"{Application.ExecutablePath.Split('\\')[Application.ExecutablePath.Split('\\').Count() - 1]}", "") + "Logs.txt";
        private static bool CheckFile()
        {
            return File.Exists(FilePath);
        }
        private static void MakeFile()
        {
            switch (CheckFile())
            {
                case false: File.Create(FilePath).Dispose(); break;
            }
        }
        public static void Log(string Gamertag, string ExternalIP, string XUID)
        {
            MakeFile();
            if (!File.ReadAllText(FilePath).Contains($"{Gamertag}:{ExternalIP}:{XUID}"))
            {
                File.AppendAllText(FilePath, $"{Gamertag}:{ExternalIP}:{XUID}{Environment.NewLine}");
            }
        }
        public static void ReadLogs()
        {
            MakeFile();
            LogsInfoGrabbed = new BindingList<LogsInfo>();
            string[] logs = File.ReadAllLines(FilePath);
            for (int i = 0; i < logs.Count(); i++)
            {
                Application.DoEvents();
                string[] Splitter = logs[i].Split(':');
                string Gamertag = Splitter[0];
                string ExternalIP = Splitter[1];
                string XUID = Splitter[2];
                LogsInfoGrabbed.Add(new LogsInfo(Gamertag, ExternalIP, XUID));
            }
        }
    }
}
