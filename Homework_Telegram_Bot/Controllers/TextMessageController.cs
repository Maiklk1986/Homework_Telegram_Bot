using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Homework_Telegram_Bot.Servises;


namespace Homework_Telegram_Bot.Controllers
{
    class TextMessageController
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly IStorage _memoryStorage;

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            this.telegramBotClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            if (message.Text == "/start")
            {
                // Объект, представляющий кнопки
                var buttons = new List<InlineKeyboardButton[]>();
                buttons.Add(new[]
                {
                  InlineKeyboardButton.WithCallbackData($" Подсчет символов" , $"len"),
                  InlineKeyboardButton.WithCallbackData($" Сложение чисел" , $"summ")
              });

                // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                await telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Наш бот подсчитывает длину введенного сообщения или складыает введенные числа</b> {Environment.NewLine}" +
                    $"{Environment.NewLine}Выберите необходимое действие.{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
            }
            else if (_memoryStorage.GetSession(message.Chat.Id).TypeOfOperation == "len")
            {

                await telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"В строке {message.Text}</b>" + $"{message.Text.Length.ToString()} символов", cancellationToken: ct);
            }
            else if (_memoryStorage.GetSession(message.Chat.Id).TypeOfOperation == "summ")
            {
                string str = message.Text;
                string[] numbers = str.Split(' ');
                int[] nums = new int[numbers.Length];
                try
                {
                    for (int i = 0; i < numbers.Length; i++)
                    {
                        nums[i] = int.Parse(numbers[i]);
                    }
                     int sum = 0;
                foreach (int i in nums)
                {
                    sum += i;
                }
                await telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Сумма чисел из введенной вами строки равна: {sum}", cancellationToken: ct);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Исключение {ex}");
                    await telegramBotClient.SendTextMessageAsync(message.Chat.Id, "Неверный формат строки", cancellationToken: ct);
                }
               
            }


        }


    }
}
