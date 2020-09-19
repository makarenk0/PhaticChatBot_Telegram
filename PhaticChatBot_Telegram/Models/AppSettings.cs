using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhaticChatBot_Telegram.Models
{
    static public class AppSettings
    {
        public static string Url { get; set; } = ""; //url to server

        public static string Name { get; set; } = ""; //bot name

        public static string Key { get; set; } = ""; //telegram api key
    }
}