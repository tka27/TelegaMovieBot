using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace TelegaBot;

public class BotManager
{
    public static void BotInit()
    {
        //"5773085059:AAHb24bE4cIymAuaTRnWSRKy3CHx8xgDbAE"
        string botID = File.ReadAllText(@"BotID.txt");
        var botClient = new TelegramBotClient(botID);
        var options = new ReceiverOptions();
        options.ThrowPendingUpdates = true;
        botClient.StartReceiving(UpdateAsync, ErrorAsync, options);
    }

    private static async Task UpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
    {
        var message = update.Message;
        if (message?.Text != null)
        {
            if (message.Text.Contains("youtube") || message.Text.Contains("jut.su"))
            {
                BrowserManager.LastURL = message.Text;
                var keyboard = BotKeyboard.CreateYoutubeKeyboard();
                await client.SendTextMessageAsync(message.Chat.Id, "Keyboard", replyMarkup: keyboard);

                BrowserManager.OpenUrl(message.Text);
            }
        }
        else if (update.CallbackQuery != null)
        {
            BotKeyboard.OnButtonRequest(update.CallbackQuery.Data);
        }
    }

    private static Task ErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        Console.WriteLine("Error");
        BrowserManager.CloseBrowser();
        return Task.CompletedTask;
    }
}