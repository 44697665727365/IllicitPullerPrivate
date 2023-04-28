using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IllicitPullerPrivate.Classes
{
    using static Connection;

    class GameDetector
    {
        public static uint MaxClients = 0;
        public static bool GrabParty = false;
        public static Games GameSelected = Games.NONE;
        public static List<uint[]> StoringOffsets = new List<uint[]>();
        public static uint[] GrabbingOffsets = new uint[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        public enum Games
        {
            NONE,
            MW2,
            BO1,
            MW3,
            BO2,
            Ghosts,
            AW,
            GTA
        }

        //Pres, Level, Clantag, Gamertag, External, Internal, Port, NPID, Mic, Nat, XUID, Party ID | PreGame
        //Pres, Level, Clantag, Gamertag, External, Internal, Port, NPID, Mic, Nat, XUID, Party ID | Party
        //Party Detector, Interval | Helpers
        //startOfs, lenForIp, lenForVerify | Spoofer
        //Server Area, Name Changer, Reset Name Changer, XUID Spoofer | Extra Mods
        public static void DetectGame()
        {
            GrabbingOffsets = new uint[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            string MW2Regions = "BLUS30377*BLES00683";
            string MW3Regions = "BLUS30838*BLES01428";
            string BO1Regions = "BLUS30591*BLES01031*BLES01032*BLES01033";
            string BO2Regions = "BLUS31011*BLES01717*BLES01718*BLES01719*BLES01720";
            string GHOSTSRegions = "BLUS31270*BLES301945*BLES31245*BLES31248*CUSA00018*NPEB00980";
            string AWRegions = "BLUS31466*BLES02077*BLES02079*CUSA00803";
            string GTA5Regions = "BLUS31156*BLES01807";
            string MemoryRead = CFW.Extension.ReadString(0x10010251);
            string MemoryReadAW = CFW.Extension.ReadString(0x20010251);
            if (MemoryRead == "" && MemoryReadAW.Contains(AWRegions))
            {
                GameSelected = Games.AW;
                StoringOffsets = new List<uint[]>();
                StoringOffsets.Add(new uint[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                StoringOffsets.Add(new uint[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                StoringOffsets.Add(new uint[] { 0x22F46F0, 0x380 });
                StoringOffsets.Add(new uint[] { 28966912, 1050000, 1000 });
                StoringOffsets.Add(new uint[] { 0xDD9CBD, 0x02A80BE8, 0x01BA273C, 0x00 });
                MaxClients = 0x12;
                return;
            }
            else
            {
                if (MemoryRead == "" && MemoryReadAW == "")
                {
                    GameSelected = Games.NONE;
                    StoringOffsets = new List<uint[]>();
                    StoringOffsets.Add(new uint[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    StoringOffsets.Add(new uint[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    StoringOffsets.Add(new uint[] { 0x00, 0x00 });
                    StoringOffsets.Add(new uint[] { 0x00, 0x00, 0x00 });
                    StoringOffsets.Add(new uint[] { 0x00, 0x00, 0x00, 0x00 });
                    return;
                }
                if (MW2Regions.Contains(MemoryRead))
                {
                    GameSelected = Games.MW2;
                    StoringOffsets = new List<uint[]>();
                    StoringOffsets.Add(new uint[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    StoringOffsets.Add(new uint[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    StoringOffsets.Add(new uint[] { 0xA12A7B, 0xD8 });
                    StoringOffsets.Add(new uint[] { 981467136, 1050000, 15000 });
                    StoringOffsets.Add(new uint[] { 0x9AA2D9, 0x01F9F11C, 0x0203B4FD, 0x01F9F140 });
                    MaxClients = 0x12;
                    return;
                }
                if (BO1Regions.Contains(MemoryRead))
                {
                    GameSelected = Games.BO1;
                    StoringOffsets = new List<uint[]>();
                    StoringOffsets.Add(new uint[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    StoringOffsets.Add(new uint[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    StoringOffsets.Add(new uint[] { 0xD5F7AB, 0x220 });
                    StoringOffsets.Add(new uint[] { 20971520, 1050000, 15000 });
                    StoringOffsets.Add(new uint[] { 0xD28A19, 0x02000934, 0x014C1EE4, 0x02000A38 });
                    MaxClients = 0x12;
                    return;
                }
                if (MW3Regions.Contains(MemoryRead))
                {
                    GameSelected = Games.MW3;
                    StoringOffsets = new List<uint[]>();
                    StoringOffsets.Add(new uint[] { 0x0089D190, 0x0089D18F, 0x0089D1F0, 0x0089D1AC, 0x0089D29E, 0x0089D28C, 0x0089D2A2, 0x0089D1C2, 0x0089D1F5, 0x0089D2A4, 0x0089D2C8, 0x0089D87E });
                    StoringOffsets.Add(new uint[] { 0x008A80E8, 0x008A80E7, 0x008A8148, 0x008A8104, 0x008A81F6, 0x008A81E4, 0x008A81FA, 0x008A811A, 0x008A814D, 0x008A81FC, 0x008A8220, 0x00 });
                    StoringOffsets.Add(new uint[] { 0x8AA267, 0x178 });
                    StoringOffsets.Add(new uint[] { 805306368, 1050000, 15000 });
                    StoringOffsets.Add(new uint[] { 0x8360D5, 0x01BBBC2C, 0x00731460, 0x01BBBC50 });
                    MaxClients = 0x12;
                    return;
                }
                if (BO2Regions.Contains(MemoryRead))
                {
                    GameSelected = Games.BO2;
                    StoringOffsets = new List<uint[]>();
                    StoringOffsets.Add(new uint[] { 0x00F9E740, 0x00F9E73F, 0x00F9E6E4, 0x00F9E698, 0x00F9E726, 0x00F9E708, 0x00F9E72A, 0x00F9E6D4, 0x00F9E67D, 0x00F9E68B, 0x00F9E690, 0x00F9E734 });
                    StoringOffsets.Add(new uint[] { 0x00FA9A48, 0x00FA9A47, 0x00FA99EC, 0x00FA99A0, 0x00FA9A2E, 0x00FA9A10, 0x00FA9A32, 0x00FA99DC, 0x00FA9985, 0x00FA9993, 0x00FA9998, 0x00FA9A3C });
                    StoringOffsets.Add(new uint[] { 0x00FB2D0F, 0x148 });
                    StoringOffsets.Add(new uint[] { 27262976, 1050000, 15000 });
                    StoringOffsets.Add(new uint[] { 0xF57FC5, 0x026C0658, 0x01A2940B, 0x026C06E0 });
                    MaxClients = 0x12;
                    return;
                }
                if (GHOSTSRegions.Contains(MemoryRead))
                {
                    GameSelected = Games.Ghosts;
                    StoringOffsets = new List<uint[]>();
                    StoringOffsets.Add(new uint[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    StoringOffsets.Add(new uint[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    StoringOffsets.Add(new uint[] { 0x202B4E8, 0x1E8 });
                    StoringOffsets.Add(new uint[] { 16687104, 1050000, 1000 });
                    StoringOffsets.Add(new uint[] { 0x20ACF2D, 0x0177A238, 0x00FED594, 0x00 });
                    MaxClients = 0x12;
                    return;
                }
                if (GTA5Regions.Contains(MemoryRead))
                {
                    GameSelected = Games.GTA;
                    StoringOffsets = new List<uint[]>();
                    StoringOffsets.Add(new uint[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    StoringOffsets.Add(new uint[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    StoringOffsets.Add(new uint[] { 0x00, 0x00 });
                    StoringOffsets.Add(new uint[] { 0x00, 0x00, 0x00 });
                    StoringOffsets.Add(new uint[] { 0x00, 0x00, 0x00, 0x00 });
                    MaxClients = 0x14;
                    return;
                }
                GameSelected = Games.NONE;
                StoringOffsets = new List<uint[]>();
                StoringOffsets.Add(new uint[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                StoringOffsets.Add(new uint[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                StoringOffsets.Add(new uint[] { 0x00, 0x00 });
                StoringOffsets.Add(new uint[] { 0x00, 0x00, 0x00 });
                StoringOffsets.Add(new uint[] { 0x00, 0x00, 0x00, 0x00 });
                return;
            }
        }

        public static void UpdateGrabbingOffsets(int client)
        {
            List<uint> Temp = new List<uint>();
            switch (CFW.Extension.ReadBool(StoringOffsets[2][0]))
            {
                case true:
                    GrabbingOffsets = StoringOffsets[1];
                    break;
                case false:
                    GrabbingOffsets = StoringOffsets[0];
                    break;
            }
            foreach (uint Offset in GrabbingOffsets)
            {
                Temp.Add(Offset + (uint)client * StoringOffsets[2][1]);
            }
            GrabbingOffsets = new uint[] { Temp[0], Temp[1], Temp[2], Temp[3], Temp[4], Temp[5], Temp[6], Temp[7], Temp[8], Temp[9], Temp[10], Temp[11] };
        }
    }
}
