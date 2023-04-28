using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IllicitPullerPrivate.Classes
{
    using System.ComponentModel;
    using System.Threading;
    using static Connection;
    using static GameDetector;

    class NameChanger
    {
        public static int SkipDelay = 1;//speed
        public static bool AutoGT = false;
        public static bool FlashGT = false;
        private static BackgroundWorker NameWorker;
        public static string CurrentGT = string.Empty;
        public static string ResetGamertag = string.Empty;

        public static void InitializeName()
        {
            NameWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
            NameWorker.DoWork += NameWorker_DoWork;
            if (!NameWorker.IsBusy)
            {
                NameWorker.RunWorkerAsync();
            }
        }

        private static void NameWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Connect();
            for (; ; )
            {
                if (IsConnected && GameSelected != Games.NONE)
                {
                    if (AutoGT)
                    {
                        SetGamertag(CurrentGT);
                    }
                    if (FlashGT)
                    {
                        SetGamertag($"^{new Random().Next(0, 7)}{CurrentGT}");
                    }
                }
                Thread.Sleep(SkipDelay);
            }
        }

        public static void SetGamertag(string GT)
        {
            CFW.Extension.WriteString(StoringOffsets[4][1], GT);//Pregame
            //CFW.Extension.WriteString(StoringOffsets[4][2], GT);//Ingame
        }

        public static void GrabOriginalGamertag()
        {
            ResetGamertag = CFW.Extension.ReadString(StoringOffsets[4][2]);
        }

        public static void SetXuid(string XUID)
        {
            CFW.SetMemory(StoringOffsets[4][3], hexString(XUID));
        }

        private static byte[] hexString(string hex)
        {
            return (from x in Enumerable.Range(0, hex.Length)
                    where x % 2 == 0
                    select Convert.ToByte(hex.Substring(x, 2), 16)).ToArray<byte>();
        }
    }
}
