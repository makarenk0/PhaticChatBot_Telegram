using Telegram.Bot;
using Telegram.Bot.Types;


namespace PhaticChatBot_Telegram.Models.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }

        public abstract void Execute(Message message, TelegramBotClient client);

        public virtual bool Contains(string command)
        {
            return command.Contains(this.Name);
        }
    }
}