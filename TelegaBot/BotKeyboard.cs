using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using WindowsInput;
using WindowsInput.Native;

namespace TelegaBot;

public static class BotKeyboard
{
    private const string UpKey = "🔊";
    private const string DownKey = "🔉";
    private const string LeftKey = "⏪";
    private const string RightKey = "⏩";
    private const string PauseKey = "⏯";
    private const string CloseKey = "❌";
    private const string SkipKey = "⏭";
    private const string Quality720 = "720";
    private const string Quality1080 = "1080";


    public static InlineKeyboardMarkup CreateYoutubeKeyboard()
    {
        var keyboard = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new[]
        {
            new[] // first row
            {
                InlineKeyboardButton.WithCallbackData(PauseKey),
                InlineKeyboardButton.WithCallbackData(UpKey),
                InlineKeyboardButton.WithCallbackData(CloseKey)
            },
            new[] // second row
            {
                InlineKeyboardButton.WithCallbackData(LeftKey),
                InlineKeyboardButton.WithCallbackData(DownKey),
                InlineKeyboardButton.WithCallbackData(RightKey)
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(SkipKey),
                InlineKeyboardButton.WithCallbackData(Quality720),
                InlineKeyboardButton.WithCallbackData(Quality1080)
            }
        });
        return keyboard;
    }

    public static void OnButtonRequest(string data)
    {
        switch (data)
        {
            case PauseKey:
                BrowserManager.PlayPause();
                //windowsKeyboard.KeyDown(VirtualKeyCode.SPACE);
                return;

            case UpKey:
                BrowserManager.Keyboard.KeyDown(VirtualKeyCode.UP);
                return;

            case CloseKey:
                // BrowserManager.Keyboard.KeyDown(VirtualKeyCode.UP);
                // BrowserManager.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_W);
                BrowserManager.CloseBrowser();
                return;

            case LeftKey:
                BrowserManager.Keyboard.KeyDown(VirtualKeyCode.LEFT);
                return;

            case DownKey:
                BrowserManager.Keyboard.KeyDown(VirtualKeyCode.DOWN);
                return;

            case RightKey:
                BrowserManager.Keyboard.KeyDown(VirtualKeyCode.RIGHT);
                return;

            case SkipKey:
                BrowserManager.Skip();
                return;

            case Quality720:
                BrowserManager.ChangeQuality(Quality720);
                return;

            case Quality1080:
                BrowserManager.ChangeQuality(Quality1080);
                return;
        }
    }
}