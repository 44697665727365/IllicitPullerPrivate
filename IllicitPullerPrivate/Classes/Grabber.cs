using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IllicitPullerPrivate.Classes
{
    using System.Windows.Forms;
    using static Forms.frmMain;
    using static Connection;
    using static GameDetector;
    using static LogsLib;
    using System.Diagnostics;

    class Grabber
    {
        public static int ClientsGrabbed = 0;
        public static string[] Blacklist = new string[] { "EMPTY", "EMPTY" };

        public static void GrabClientsOnce(DataGridView DataGridView)
        {
            GrabClients(DataGridView);
            GrabOnce.Abort();
        }

        private static void GrabClients(DataGridView DataGridView)
        {
            ClientsGrabbed = 0;
            if (GameSelected == Games.NONE)
            {
                DetectGame();
            }
            try
            {
                for (int i = 0; i < (int)MaxClients; i++)
                {
                    UpdateGrabbingOffsets(i);

                    string Prestige = string.Empty;
                    string Rank = string.Empty;
                    string Clan = string.Empty;
                    string Gamertag = ReadGamertag();
                    string External = string.Empty;
                    string Internal = string.Empty;
                    string Port = string.Empty;
                    string Npid = string.Empty;
                    string Mic = string.Empty;
                    string Nat = string.Empty;
                    string Xuid = string.Empty;
                    string PartyID = string.Empty;

                    if (Gamertag != "N/A")
                    {
                        if (IsBlacklisted(Gamertag))
                        {
                            Prestige = "Who?";
                            Rank = "Who?";
                            Clan = "Who?";
                            External = "Who?";
                            Internal = "Who?";
                            Port = "Who?";
                            Npid = "Who?";
                            Mic = "Who?";
                            Nat = "Who?";
                            Xuid = "Who?";
                            PartyID = "Who?";
                        }
                        else
                        {
                            if (GameSelected != Games.GTA)
                            {
                                Prestige = ReadPrestige();
                                Rank = ReadRank();
                                Mic = ReadMic();
                                Nat = ReadNat();
                                PartyID = ReadPartyID();
                            }
                            else
                            {
                                Prestige = "N/A";
                                Rank = "N/A";
                                Mic = "N/A";
                                Nat = "N/A";
                                PartyID = "N/A";
                            }
                            Clan = ReadClan();
                            External = ReadExternal();
                            Internal = ReadInternal();
                            Port = ReadPort();
                            Npid = ReadNpid();
                            Xuid = ReadXuid();
                        }

                        if (External != "N/A" && Internal != "N/A" && Port != "N/A")
                        {
                            ClientsGrabbed = ClientsGrabbed + 1;
                            Application.DoEvents();
                            if (GameSelected != Games.GTA)
                            {
                                DataGridView[0, i].Value = Prestige;
                                DataGridView[1, i].Value = Rank;
                                DataGridView[2, i].Value = Clan;
                            }
                            else
                            {
                                DataGridView[0, i].Value = "N/A";
                                DataGridView[1, i].Value = "N/A";
                                DataGridView[2, i].Value = "N/A";
                            }
                            DataGridView[3, i].Value = Gamertag;
                            DataGridView[4, i].Value = External;
                            DataGridView[5, i].Value = Internal;
                            DataGridView[6, i].Value = Port;
                            DataGridView[7, i].Value = Npid;
                            DataGridView[8, i].Value = Mic;
                            DataGridView[9, i].Value = Nat;
                            DataGridView[10, i].Value = Xuid;
                            DataGridView[11, i].Value = PartyID;

                            Log(Gamertag, External, Xuid);
                        }
                        else
                        {
                            DataGridView[0, i].Value = string.Empty;
                            DataGridView[1, i].Value = string.Empty;
                            DataGridView[2, i].Value = string.Empty;
                            DataGridView[3, i].Value = string.Empty;
                            DataGridView[4, i].Value = string.Empty;
                            DataGridView[5, i].Value = string.Empty;
                            DataGridView[6, i].Value = string.Empty;
                            DataGridView[7, i].Value = string.Empty;
                            DataGridView[8, i].Value = string.Empty;
                            DataGridView[9, i].Value = string.Empty;
                            DataGridView[10, i].Value = string.Empty;
                            DataGridView[11, i].Value = string.Empty;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private static bool IsBlacklisted(string name) => Blacklist.Any((string s) => name.Contains(s));

        private static string ReadPrestige()
        {
            byte[] input = CFW.Extension.ReadBytes(GrabbingOffsets[0], 0x04);
            string Prestige = BitConverter.IsLittleEndian ? BitConverter.ToInt32(new byte[] { input[3], input[2], input[1], input[0] }, 0).ToString() : BitConverter.ToInt32(input, 0).ToString();
            return Prestige;
        }

        private static string ReadRank()
        {
            byte input = CFW.Extension.ReadByte(GrabbingOffsets[1]);
            return Convert.ToByte(input + 0x1).ToString();
        }

        private static string ReadClan()
        {
            string MemorySaver = CFW.Extension.ReadString(GrabbingOffsets[2]);
            string StringRead = string.IsNullOrEmpty(MemorySaver) ? "N/A" : MemorySaver;
            switch (StringRead)
            {
                case null: return "N/A";
                case "": return "N/A";
                default: return StringRead;
            }
        }

        private static string ReadGamertag()
        {
            string MemorySaver = CFW.Extension.ReadString(GrabbingOffsets[3]);
            string StringRead = string.IsNullOrEmpty(MemorySaver) ? "N/A" : MemorySaver;
            switch (StringRead)
            {
                case null: return "N/A";
                case "": return "N/A";
                default: return StringRead;
            }
        }

        private static string ReadExternal()
        {
            byte[] input = CFW.Extension.ReadBytes(GrabbingOffsets[4], 0x04);
            string IP = $"{input[0]}.{input[1]}.{input[2]}.{input[3]}";
            switch (IP)
            {
                case "0.0.0.0": return "N/A";
                case "1.0.1.0": return "N/A";
                case "0.0.255.0": return "N/A";
                case "0.255.0.255": return "N/A";
                case "255.0.255.0": return "N/A";
                case "255.0.255.255": return "N/A";
                case "255.255.255.255": return "N/A";
                default: return IP;
            }
        }

        private static string ReadInternal()
        {
            byte[] input = CFW.Extension.ReadBytes(GrabbingOffsets[5], 0x04);
            string IP = $"{input[0]}.{input[1]}.{input[2]}.{input[3]}";
            switch (IP)
            {
                case "0.0.0.0": return "N/A";
                case "1.0.1.0": return "N/A";
                case "0.0.255.0": return "N/A";
                case "0.255.0.255": return "N/A";
                case "255.0.255.0": return "N/A";
                case "255.0.255.255": return "N/A";
                case "255.255.255.255": return "N/A";
                default: return IP;
            }
        }

        private static string ReadPort()
        {
            byte[] input = CFW.Extension.ReadBytes(GrabbingOffsets[6], 0x02);
            string Port = (input[1] << 8 | input[0]).ToString();
            switch (Port)
            {
                case "0": return "N/A";
                case "65535": return "N/A";
                default: return Port;
            }
        }

        private static string ReadNpid()
        {
            string MemorySaver = CFW.Extension.ReadString(GrabbingOffsets[7]);
            string StringRead = string.IsNullOrEmpty(MemorySaver) ? "N/A" : MemorySaver;
            switch (StringRead)
            {
                case null: return "N/A";
                case "": return "N/A";
                default: return StringRead;
            }
        }

        private static string ReadMic() => CFW.Extension.ReadByte(GrabbingOffsets[8]) == 0x00 ? "OFF" : "ON";

        private static string ReadNat()
        {
            byte input = CFW.Extension.ReadByte(GrabbingOffsets[9]);
            switch (input)
            {
                case 0x0: return "N/A";
                case 0x1: return "Open";
                case 0x2: return "Moderate";
                case 0x3: return "Strict";
                default: return "N/A";
            }
        }

        private static string ReadXuid()
        {
            byte[] input = CFW.Extension.ReadBytes(GrabbingOffsets[10], 0x08);
            string Xuid = BitConverter.ToString(input).Replace("-", "");
            switch (Xuid)
            {
                case "00000000000000000": return "N/A";
                default: return Xuid;
            }
        }

        private static string ReadPartyID()
        {
            byte[] input = CFW.Extension.ReadBytes(GrabbingOffsets[11], 0x08);
            string PartyId = BitConverter.ToString(input).Replace("-", "");
            switch (PartyId)
            {
                case "00000000000000000": return "N/A";
                default: return PartyId;
            }
        }
    }
}
