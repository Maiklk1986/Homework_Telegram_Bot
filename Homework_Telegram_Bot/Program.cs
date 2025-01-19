using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Text;
using Telegram.Bot;
using Homework_Telegram_Bot.Controllers;
using Homework_Telegram_Bot.Configuration;
using Homework_Telegram_Bot.Servises;

namespace Homework_Telegram_Bot
{
    class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build(); // Собираем

            Console.WriteLine("Сервис запущен");
            // Запускаем сервис
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");
        }

        static void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(appSettings);
            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.Bot_token));
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();

            //Инициализация конфигурации


            //
            services.TryAddSingleton<IStorage, MemoryStorage>();

            // Подключаем контроллеры сообщений и кнопок
            services.AddTransient<DefaultMessagecontroller>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<InlineKeyboardController>();
        }


          

        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
                Bot_token = "8178934062:AAH3_cjKuoER3Hwi6deevVFtRMtIf25l_nw",               
            };
        }
    }
}
