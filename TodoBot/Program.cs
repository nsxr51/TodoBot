

using System;
using System.Collections.Generic;
using System.Threading;

namespace Telegram.Bot.Examples.Echo
{
    public static class Program
    {


        public static void Main(string[] args)
        {
 
            TelegramBot tg = null;
            try
            {
                tg = new TelegramBot();
                tg.Start();
                Thread.Sleep(-1);
            }
            catch (Exception ex)
            {
                tg.Stop();
            }

        }


    }
}
