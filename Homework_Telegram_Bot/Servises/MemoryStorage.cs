using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Homework_Telegram_Bot.Configuration;


namespace Homework_Telegram_Bot.Servises
{
    class MemoryStorage : IStorage
    {
        /// <summary>
        /// Хранилище сессий
        /// </summary>
        private readonly ConcurrentDictionary<long, Session> _sessions;

        public MemoryStorage()
        {
            _sessions = new ConcurrentDictionary<long, Session>();
        }

        public Session GetSession(long chatId)
        {
            // Возвращаем сессию по ключу, если она существует
            if (_sessions.ContainsKey(chatId))
                return _sessions[chatId];

            // Создаем и возвращаем новую, если такой не было
            var newSession = new Session();
            _sessions.TryAdd(chatId, newSession);
            return newSession;
        }
    }

    public class Session
    {
        public string TypeOfOperation { get; set; }

        public Session()
            {
            TypeOfOperation = "len";
            }
        public Session(string str)
        {
            TypeOfOperation = str;
        }


    }
}
