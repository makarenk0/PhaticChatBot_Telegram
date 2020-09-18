using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhaticChatBot_Telegram.Models
{
    static public class AppSettings
    {
        public static string Url { get; set; } = "https://phaticchatbottelegram.azurewebsites.net:443/{0}";

        public static string Name { get; set; } = "PhaticChatBot";

        public static string Key { get; set; } = "1200498122:AAEYN_smwdCZC8SP8DIofYeV3Bc1BXVHNSg";
    }
}