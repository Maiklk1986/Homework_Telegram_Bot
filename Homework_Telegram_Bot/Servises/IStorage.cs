﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_Telegram_Bot.Servises
{
    interface IStorage
    {
        Session GetSession(long chatId);
    }
}
