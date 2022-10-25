namespace TelegaBot;

internal class Program
{
    public static void Main(string[] args)
    {
        BrowserManager.InputSimulator = new();
        BotManager.BotInit();
        new ManualResetEvent(false).WaitOne();
    }
}