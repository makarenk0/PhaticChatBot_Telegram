using PhaticChatBot_Telegram.Models.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;

namespace PhaticChatBot_Telegram.Models
{
    static public class Bot
    {
        public static TelegramBotClient client;


        private static List<Command> _commandsList;
        public static IReadOnlyList<Command> Commands 
        {
            get { return _commandsList; }
        }

        public static TelegramBotClient Get()
        {
            if (client != null)
            {
                return client;
            }

            _commandsList = new List<Command>();
            _commandsList.Add(new SpeechCommand(HttpContext.Current.Server.MapPath("/App_Data/patterns.json")));

            client = new TelegramBotClient(AppSettings.Key);
            var hook = string.Format(AppSettings.Url, "api/message/update");
            client.SetWebhookAsync(hook);

            return client;
        }
    }
}