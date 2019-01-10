using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Examples.Echo
{
    class TelegramBot
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("768480933:AAHzrjaO8-hDclAGKuAj5yK9BbjPGdyjz_A");
        private static MessagesHandler _msgHandler=new MessagesHandler();
        public TelegramBot()
        {
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnInlineQuery += BotOnInlineQueryReceived;
            Bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            Bot.OnReceiveError += BotOnReceiveError;
        }

        public void Start()
        {
            Bot.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine($"Start listening for @{Bot.GetMeAsync().Result}");
        }
        public void Stop()
        {
            Bot.StopReceiving();
            Console.WriteLine($"Stop listening for @{Bot.GetMeAsync().Result}");
        }
        #region BotEvents
        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            Task.Run(() =>
            {
                HandleBotMessage(messageEventArgs);
            });
         
        }

        private static async void HandleBotMessage(MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            Console.WriteLine($"recived message id{message.Chat.Id}");
            if (message == null || message.Type != MessageType.Text) return;
            if (message.From.Username != "Liorbennaim" && message.From.Id != 723715194) return;
            if (_msgHandler.IsMessageInDictionary(message.Chat.Id))
            {
                var msg = _msgHandler.GetMessageFromDictionary(message.Chat.Id);
                msg.ParseMessage(message.Text, Bot);
            }
            else
            {
                _msgHandler.AddMessageToList(message.Chat.Id);
                var msg = _msgHandler.GetMessageFromDictionary(message.Chat.Id);
                msg.ParseMessage(message.Text, Bot);
            }
            return;
            switch (message.Text)
            {
                // send inline keyboard
                case "/inline":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

                    await Task.Delay(500); // simulate longer running task

                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new [] // first row
                        {
                            InlineKeyboardButton.WithCallbackData("1.1"),
                            InlineKeyboardButton.WithCallbackData("1.2"),
                        }

                    });

                    await Bot.SendTextMessageAsync(
                        message.Chat.Id,
                        "Choose",
                        replyMarkup: inlineKeyboard);
                    break;

                // send custom keyboard
                case "/keyboard":
                    ReplyKeyboardMarkup ReplyKeyboard = new[]
                    {
                        new[] { "1.1", "1.2" },
                        new[] { "2.1", "2.2" },
                    };

                    await Bot.SendTextMessageAsync(
                        message.Chat.Id,
                        "Choose",
                        replyMarkup: ReplyKeyboard);
                    break;

                // send a photo
                case "שלח":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

                    const string file = @"Files/test2.pdf";

                    var fileName = file.Split(Path.DirectorySeparatorChar).Last();
                    using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        await Bot.SendDocumentAsync(message.Chat.Id, new Telegram.Bot.Types.InputFiles.InputOnlineFile(fileStream, fileName));
                        //await Bot.SendDocumentAsync(
                        //    message.Chat.Id,
                        //    fileStream,
                        //    "Nice Picture");
                    }
                    break;
                case "קניות בסופר":
                    ReplyKeyboardMarkup ReplyKeyboardnAmount = new[]
                    {
                        new[] {"7", "8", "9" },
                        new[] { "4", "5", "6" },
                        new[] { "1", "2", "3" },
                        new[] {"*","0","." },
                    };
                    await Bot.SendTextMessageAsync(
                        message.Chat.Id,
                        "הכנס סכום",
                        replyMarkup: ReplyKeyboardnAmount);
                    break;
                // request location or contact
                case "הוצאה חדשה":
              
                    break;
       
            }
        }

        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;

            await Bot.AnswerCallbackQueryAsync(
                callbackQuery.Id,
                $"Received {callbackQuery.Data}");

            await Bot.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                $"Received {callbackQuery.Data}");
        }

        private static async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
            Console.WriteLine($"Received inline query from: {inlineQueryEventArgs.InlineQuery.From.Id}");

            InlineQueryResultBase[] results = {
                new InlineQueryResultLocation(
                    id: "1",
                    latitude: 40.7058316f,
                    longitude: -74.2581888f,
                    title: "New York")   // displayed result
                    {
                        InputMessageContent = new InputLocationMessageContent(
                            latitude: 40.7058316f,
                            longitude: -74.2581888f)    // message if result is selected
                    },

                new InlineQueryResultLocation(
                    id: "2",
                    latitude: 13.1449577f,
                    longitude: 52.507629f,
                    title: "Berlin") // displayed result
                    {

                        InputMessageContent = new InputLocationMessageContent(
                            latitude: 13.1449577f,
                            longitude: 52.507629f)   // message if result is selected
                    }
            };

            await Bot.AnswerInlineQueryAsync(
                inlineQueryEventArgs.InlineQuery.Id,
                results,
                isPersonal: true,
                cacheTime: 0);
        }

        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("Received error: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message);
        }
#endregion
    }
}
