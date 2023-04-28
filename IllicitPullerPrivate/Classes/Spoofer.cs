namespace IllicitPullerPrivate.Classes
{
    using System;
    using System.Net;
    using static Connection;
    using static GameDetector;
    class Spoofer
    {
        internal static uint IPSpoofAddr = 0x0, PortSpoofAddr = 0x0;
        static byte[] IpToFind = new byte[] { 0x0, 0x0, 0x0, 0x0 };
        internal static byte[] StoredIPB = new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
        static string OriginalIP = new WebClient { Proxy = null }.DownloadString("http://checkip.dyndns.org/").Replace("Current IP Address: ", string.Empty);

        private static byte[] GrabIpByte(string ip) => new byte[] { Convert.ToByte(ip.Split('.')[0]), Convert.ToByte(ip.Split('.')[1]), Convert.ToByte(ip.Split('.')[2]), Convert.ToByte(ip.Split('.')[3]) };

        private static string GrabIpString(byte[] ip) => $"{ip[0]}.{ip[1]}.{ip[2]}.{ip[3]}";

        public static void SpoofIP(string ip, string port)
        {
            if (IsConnected)
            {
                CFW.Extension.WriteBytes(IPSpoofAddr, GrabIpByte(ip));
                if (GameSelected == Games.Ghosts | GameSelected == Games.AW)
                {
                    CFW.Extension.WriteUInt16(PortSpoofAddr, Convert.ToUInt16(port));
                }
                else
                {
                    CFW.Extension.WriteUInt16(IPSpoofAddr + 4, Convert.ToUInt16(port));
                }
            }
        }
        public static void ResetSpoofIP()
        {
            if (IsConnected)
            {
                byte[] OgIP = GrabIpByte(OriginalIP);
                if (IpToFind == OgIP)
                {
                    CFW.Extension.WriteBytes(IPSpoofAddr, IpToFind);
                }
                else
                {
                    CFW.Extension.WriteBytes(IPSpoofAddr, OgIP);
                }
                if (GameSelected == Games.Ghosts | GameSelected == Games.AW)
                {
                    CFW.Extension.WriteUInt16(PortSpoofAddr, Convert.ToUInt16("3074"));
                }
                else
                {
                    CFW.Extension.WriteUInt16(IPSpoofAddr + 4, Convert.ToUInt16("3074"));
                }
            }
        }
        public static bool SetSpoofAddrs()
        {
            if (IsConnected)
            {
                byte[] port = new byte[] { 0x00, 0x00 };
                DetectGame();
                UpdateGrabbingOffsets(0);
                IpToFind = CFW.Extension.ReadBytes(GrabbingOffsets[4], 0x04);
                CFW.Extension.ReadBytes(GrabbingOffsets[6], 0x02);
                if (port == new byte[] { 0x0, 0x0 }) { return false; }
                Array.Reverse(port);
                switch (GameSelected)
                {
                    case Games.NONE: return false;
                    case Games.GTA: return false;
                    case Games.Ghosts:
                        IPSpoofAddr = findInfo(StoringOffsets[3][0], (int)StoringOffsets[3][1], (int)StoringOffsets[3][2], IpToFind, port);
                        PortSpoofAddr = findInfo(IPSpoofAddr, 1000, 2000, port, new byte[] { 0x67, 0x68, 0x6F, 0x73, 0x74, 0x73 });
                        CFW.Extension.ReadBytes(IPSpoofAddr, 4).CopyTo(StoredIPB, 0);
                        CFW.Extension.ReadBytes(PortSpoofAddr, 2).CopyTo(StoredIPB, 4);
                        return (IPSpoofAddr == 0x00 ? false : true);
                    case Games.AW:
                        IPSpoofAddr = findInfo(StoringOffsets[3][0], (int)StoringOffsets[3][1], (int)StoringOffsets[3][2], IpToFind, port);
                        PortSpoofAddr = findInfo(IPSpoofAddr, 1000, 2000, port, new byte[] { 0x5C, 0x5C, 0x5C, 0x5C });
                        CFW.Extension.ReadBytes(IPSpoofAddr, 4).CopyTo(StoredIPB, 0);
                        CFW.Extension.ReadBytes(PortSpoofAddr, 2).CopyTo(StoredIPB, 4);
                        return (IPSpoofAddr == 0x00 ? false : true);
                    default:
                        IPSpoofAddr = findInfo(StoringOffsets[3][0], (int)StoringOffsets[3][1], (int)StoringOffsets[3][2], IpToFind, port);
                        StoredIPB = CFW.Extension.ReadBytes(IPSpoofAddr, 6);
                        return (IPSpoofAddr == 0x00 ? false : true);
                }
            }
            return false;
        }
        static uint findInfo(uint startOfs, int lenForIp, int lenForVerify, byte[] findIp, byte[] verifyIp)
        {
            byte[] search = CFW.Extension.ReadBytes(startOfs, lenForIp);
            int findIpPos = 0;

            while (true)
            {
                findIpPos = findBytes(search, findIp, findIpPos);
                if (findIpPos != -1)
                {
                    byte[] searchVerify = CFW.Extension.ReadBytes(startOfs + (uint)findIpPos, lenForVerify);
                    int findVerifyIpPos = findBytes(searchVerify, verifyIp, 0);
                    if (findVerifyIpPos != -1)
                    {
                        return startOfs + (uint)findIpPos;
                    }
                    else
                    {
                        findIpPos += findIp.Length;
                    }
                }
                else
                {
                    return 0x00;
                }
            }
        }
        static int findBytes(byte[] haystack, byte[] needle, int start_index)
        {
            int len = needle.Length;
            int limit = haystack.Length - len;
            for (int i = start_index; i <= limit; i++)
            {
                int k = 0;
                for (; k < len; k++)
                {
                    if (needle[k] != haystack[i + k]) break;
                }
                if (k == len) return i;
            }
            return -1;
        }
    }
}
