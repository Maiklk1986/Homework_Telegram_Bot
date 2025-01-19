using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using Homework_Telegram_Bot.Controllers;

namespace Homework_Telegram_Bot
{
    class Bot : BackgroundService
    {
        // Клиент к Telegram Bot API
        private ITelegramBotClient _telegramClient;

        // Контроллеры различных видов сообщений
        private InlineKeyboardController _inlineKeyboardController;
        private TextMessageController _textMessageController;
        private DefaultMessagecontroller _defaultMessageController;

        public Bot(
            ITelegramBotClient telegramClient,
            InlineKeyboardController inlineKeyboardController,
            TextMessageController textMessageController,
            DefaultMessagecontroller defaultMessageController)
        {
            _telegramClient = telegramClient;
            _inlineKeyboardController = inlineKeyboardController;
            _textMessageController = textMessageController;
            _defaultMessageController = defaultMessageController;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _telegramClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } }, // Здесь выбираем, какие обновления хотим получать. В данном случае - разрешены все
                cancellationToken: stoppingToken);

            Console.WriteLine("Бот запущен.");
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            //  Обрабатываем нажатия на кнопки  из Telegram Bot API: https://core.telegram.org/bots/api#callbackquery
            if (update.Type == UpdateType.CallbackQuery)
            {
                await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
                return;
            }

            // Обрабатываем входящие сообщения из Telegram Bot API: https://core.telegram.org/bots/api#message
            if (update.Type == UpdateType.Message)
            {
                switch (update.Message.Type)
                {

                    case MessageType.Text:
                        await _textMessageController.Handle(update.Message, cancellationToken);
                        return;
                    default:
                        await _defaultMessageController.Handle(update.Message, cancellationToken);
                        return;
                }
            }
        }

        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
           
            try
            {
                Exception exx1 = new ApiRequestException(404, "Resource not found");
               
                if(exception == exx1)
                throw new ApiRequestException(404, "Resource not found");
            }
            catch (Exception ex)
            {
                var message = ExceptionHandling.GetErrorMessage(ex);
                Console.WriteLine(message);
            }

            try
            {
                Exception exx2 = new FormatException();
                if (exception == exx2)
                    throw new FormatException("Incorrect format");
            }
            catch (Exception ex)
            {
                var message = ExceptionHandling.GetErrorMessage(ex);
                Console.WriteLine(message);

            }
            
            
            Thread.Sleep(10000);

            return Task.CompletedTask;
        }
    }

    public class ApiRequestException : Exception
    {
        public int ErrorCode { get; set; }

        public ApiRequestException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }

    public class ExceptionHandling
    {
        public static string GetErrorMessage(Exception exception)
        {
            string errorMessage;

            if (exception is ApiRequestException)
            {
                var apiRequestException = (ApiRequestException)exception; // Явное приведение типа
                errorMessage = $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}";
            }
            else
            {
                errorMessage = exception.ToString();
            }

            return errorMessage;
        }
    }
}

