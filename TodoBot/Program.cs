using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using TodoBot.Expenses;

namespace Telegram.Bot.Examples.Echo
{
    public static class Program
    {


        public static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                  .AddSingleton<IExpenses, FireBaseExpenses>()
                  .BuildServiceProvider();
            var bar = serviceProvider.GetService<IExpenses>();
            bar.LoadExpenses();
            Thread.Sleep(-1);
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
