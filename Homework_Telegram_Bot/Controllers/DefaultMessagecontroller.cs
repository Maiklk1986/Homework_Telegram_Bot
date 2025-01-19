using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace Homework_Telegram_Bot.Controllers
{
    class DefaultMessagecontroller
    {
        private readonly ITelegramBotClient telegramBotClient;

        public DefaultMessagecontroller(ITelegramBotClient telegramBotClient)
        {
            this.telegramBotClient = telegramBotClient;
        }

        public async Task Handle (Message message, CancellationToken ct)
        {
            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");
            await telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Получен незнакомый файл", cancellationToken: ct);
        }
    }
}
