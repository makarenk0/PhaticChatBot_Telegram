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

           
            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text && !String.IsNullOrEmpty(message.Text))
                client.SendTextMessageAsync(chatId, finder.GetPatternAnswer(message.Text));
            else { client.SendTextMessageAsync(chatId, "What's this?"); }


        }

        public override bool Contains(string command)
        {
            return !command.Contains("/");
        }
    }
}