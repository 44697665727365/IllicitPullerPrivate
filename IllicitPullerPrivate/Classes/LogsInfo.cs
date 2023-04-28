namespace IllicitPullerPrivate.Classes
{
    public class LogsInfo
    {
        public LogsInfo(string Gamertag, string ExternalIP, string XUID)
        {
            this.Gamertag = Gamertag;
            this.ExternalIP = ExternalIP;
            this.XUID = XUID;
        }
        public string Gamertag { get; set; }
        public string ExternalIP { get; set; }
        public string XUID { get; set; }
    }
}