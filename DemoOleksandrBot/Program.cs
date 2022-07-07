using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace DemoOleksandrBot
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var bot = new TelegramBotClient(BotConfiguration.BotToken);

            var me = await bot.GetMeAsync();
            Console.Title = me.Username ?? "Demo Bot";

            using var cts = new CancellationTokenSource();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = Array.Empty<UpdateType>(),
                ThrowPendingUpdates = true,
            };

            bot.StartReceiving(updateHandler: UpdateHandlers.HandleUpdateAsync,
                               pollingErrorHandler: UpdateHandlers.PollingErrorHandler,
                               receiverOptions: receiverOptions,
                               cancellationToken: cts.Token);

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }
    }
}
