using PhaticChatBot_Telegram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Telegram.Bot.Types;

namespace PhaticChatBot_Telegram.Controllers
{
    public class MessageController : ApiController
    {
        public OkResult Update([FromBody]Update update)
        {
            var commands = Bot.Commands;
            var message = update.Message;
            var client = Bot.Get();

            foreach (var command in commands)
            {
                if(message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                {
                    if (command.Contains(message.Text))
                    {
                        command.Execute(message, client);
                        break;
                    }
                }
                
            }
            return Ok();
        }
    }
}
