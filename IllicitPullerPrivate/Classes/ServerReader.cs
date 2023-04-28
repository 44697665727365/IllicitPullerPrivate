using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IllicitPullerPrivate.Classes
{
    using static Connection;
    using static GameDetector;

    class ServerReader
    {
        public static string Hostname = "N/A";
        public static string Mapname = "N/A";
        public static string Gamemode = "N/A";

        public static void ReadServerInfo()
        {
            if (IsConnected && GameSelected != Games.NONE)
            {
                ReadServerArea();
            }
            else
            {
                Hostname = "N/A";
                Mapname = "N/A";
                Gamemode = "N/A";
            }
        }

        private static string ReadServerArea()
        {
            string ServerArea = CFW.Extension.ReadString(StoringOffsets[4][0]);
            string[] ServerAreaSplit = null;
            switch (GameSelected)
            {
                case Games.MW2:
                    if (ServerArea != string.Empty)
                    {
                        ServerAreaSplit = ServerArea.Split(new char[] { Convert.ToChar(0x5C) });
                        Hostname = ServerAreaSplit[16];
                        Mapname = ServerAreaSplit[6];
                        Gamemode = ServerAreaSplit[2];
                    }
                    break;
                case Games.BO1:
                    if (ServerArea != string.Empty)
                    {
                        ServerAreaSplit = ServerArea.Split(new char[] { Convert.ToChar(0x5C) });
                        Hostname = ServerAreaSplit[14];
                        Mapname = ServerAreaSplit[6];
                        Gamemode = ServerAreaSplit[4];
                    }
                    break;
                case Games.MW3:
                    if (ServerArea != string.Empty)
                    {
                        ServerAreaSplit = ServerArea.Split(new char[] { Convert.ToChar(0x5C) });
                        Hostname = ServerAreaSplit[16];
                        Mapname = ServerAreaSplit[6];
                        Gamemode = ServerAreaSplit[2];
                    }
                    break;
                case Games.BO2:
                    if (ServerArea != string.Empty)
                    {
                        ServerAreaSplit = ServerArea.Split(new char[] { Convert.ToChar(0x5C) });
                        Hostname = ServerAreaSplit[14];
                        Mapname = ServerAreaSplit[8];
                        Gamemode = ServerAreaSplit[4];
                    }
                    break;
                case Games.Ghosts:
                    if (ServerArea != string.Empty)
                    {
                        ServerAreaSplit = ServerArea.Split(new char[] { Convert.ToChar(0x5C) });
                        Hostname = ServerAreaSplit[18];
                        Mapname = ServerAreaSplit[6];
                        Gamemode = ServerAreaSplit[2];
                    }
                    break;
                case Games.AW:
                    if (ServerArea != string.Empty)
                    {
                        ServerAreaSplit = ServerArea.Split(new char[] { Convert.ToChar(0x5C) });
                        Hostname = ServerAreaSplit[18];
                        Mapname = ServerAreaSplit[6];
                        Gamemode = ServerAreaSplit[2];
                    }
                    break;
            }
            return "N/A";
        }
    }
}
