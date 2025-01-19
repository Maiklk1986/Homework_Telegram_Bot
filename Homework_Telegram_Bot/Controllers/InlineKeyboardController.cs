using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Homework_Telegram_Bot.Servises;


namespace Homework_Telegram_Bot.Controllers
{
    class InlineKeyboardController
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly IStorage _memoryStorage;
       
        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            telegramBotClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(CallbackQuery callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            // Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(callbackQuery.From.Id).TypeOfOperation = callbackQuery.Data;

            // Отправляем в ответ уведомление о выборе
            await telegramBotClient.SendTextMessageAsync(callbackQuery.Id,
                $"<b>Будет выполнено - {callbackQuery.Data}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Операцию можно изменить в главном меню", cancellationToken: ct, parseMode: ParseMode.Html);

            await telegramBotClient.SendTextMessageAsync(long.Parse(callbackQuery.Id), "Необходимо ввести ряд чисел через пробел", cancellationToken: ct, parseMode: ParseMode.Html);
            if (_memoryStorage.GetSession(callbackQuery.From.Id).TypeOfOperation == "summ")
            {
                await telegramBotClient.SendTextMessageAsync(long.Parse(callbackQuery.Id), "Необходимо ввести ряд чисел через пробел", cancellationToken: ct, parseMode: ParseMode.Html);
            }
        }       
    }
}
