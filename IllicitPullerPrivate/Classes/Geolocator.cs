using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IllicitPullerPrivate.Classes
{
    class Geolocator
    {
        public static string GetTrackerDetails(string ip)
        {
            string[] resp = new Regex("\\b\\d{1,4}\\.\\d{1,4}\\.\\d{1,4}\\.\\d{1,4}\\b").IsMatch(ip) == true ? LinkResponse(ip) : new string[] { "", "", "", "", "", "", "", "" };
            return $"{resp[0]}\n{resp[1]}\n{resp[2]}\n{resp[3]}\n{resp[4]}\n{resp[5]}\n{resp[6]}";
        }
        private static string[] LinkResponse(string ip)
        {
            string[] GeoResults = new WebClient { Proxy = null }.DownloadString($"http://json.geoiplookup.io/{ip}").Replace("\"", "").Replace("\n", "").Replace("    ", "").Split(new[] { "," }, StringSplitOptions.None);
            return new string[] { ip, GeoResults[13].Replace("hostname: ", ""), GeoResults[3].Replace("country_name: ", ""), GeoResults[4].Replace("region: ", ""), GeoResults[5].Replace("city: ", ""), GeoResults[6].Replace("postal_code: ", ""), GeoResults[11].Replace("isp: ", "") };
        }
        public static string VpnDetect(string ip)
        {
            string[] GeoResults = new WebClient { Proxy = null }.DownloadString($"http://proxycheck.io/v1/{ip}&key=111111-222222-333333-444444&vpn=1&asn=0&node=0&time=0&tag=forum%20signup%20page").Replace("\"", "").Replace("\n", "").Replace("    ", "").Split(new[] { "," }, StringSplitOptions.None);
            string Res = GeoResults[1].Replace("proxy: ", "").Replace("}", "");
            switch (Res)
            {
                case "yes": return "YES";
                case "no": return "NO";
                default: return "N/A";
            }
        }
    }
}
