using PhaticChatBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PhaticChatBot_Telegram.Models.Commands
{
    public class SpeechCommand : Command
    {
        private PatternsFinder finder;

        public override string Name => "";


        public SpeechCommand(string patternsFileNameJSON)
        {
            finder = new PatternsFinder(patternsFileNameJSON);
        }


        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            if (!String.IsNullOrEmpty(message.Text))
            {
                if (!SpecialCommands(client, chatId, message.Text))
                client.SendTextMessageAsync(chatId, finder.GetPatternAnswer(message.Text));
            }
            else client.SendTextMessageAsync(chatId, "What's this?");

        }

        public override bool Contains(string command)
        {
            return !command.Contains("/");
        }

        private bool SpecialCommands(TelegramBotClient client, long chatId, string message)
        {
            if(message.Contains("Roll a dice"))
            {
                client.SendDiceAsync(chatId);
                return true;
            }
            else if(message.Contains("What time"))
            {
                client.SendTextMessageAsync(chatId, DateTime.Now.ToString());
                return true;
            }
            return false;
        }
    }
}