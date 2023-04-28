using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IllicitPullerPrivate.Classes
{
    using PS3Lib;

    class Connection
    {
        //Variables
        public static string ControlConsoleIP = string.Empty;
        public static PS3API CFW = new PS3API();
        public static bool IsConnected = false;
        //Main Functions
        public static string Connect()
        {
            switch (CFW.GetCurrentAPI())
            {
                case SelectAPI.ControlConsole:
                    if (ControlConsoleIP == string.Empty)
                    {
                        return CFW.ConnectTarget() ? CFW.AttachProcess() ? "Connected" : "Not Connected" : "Not Connected";
                    }
                    return CFW.ConnectTarget(ControlConsoleIP) ? CFW.AttachProcess() ? "Connected" : "Not Connected" : "Not Connected";
                case SelectAPI.TargetManager:
                    return CFW.ConnectTarget() ? CFW.AttachProcess() ? "Connected" : "Not Connected" : "Not Connected";
            }
            return "Not Connected";
        }
    }
}
