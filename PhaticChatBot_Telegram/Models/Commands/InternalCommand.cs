
using PhaticChatBot;
using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PhaticChatBot_Telegram.Models.Commands
{
    public class InternalCommand : Command
    {
        public override string Name => "";
        private CommandsHandler handler;
        

        public InternalCommand(string patternsFileNameJSON)
        {
            handler = new CommandsHandler(patternsFileNameJSON);
           
        }

        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            if (!String.IsNullOrEmpty(message.Text))
                client.SendTextMessageAsync(chatId, handler.HandleCommands(message.Text.Replace("/", "")));
            else client.SendTextMessageAsync(chatId, "What's this?");

            
        }

        public override bool Contains(string command)
        {
            return command.Contains("/");
        }
    }
}