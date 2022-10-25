using System.Runtime.InteropServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using WindowsInput;
using WindowsInput.Native;

namespace TelegaBot;

public static class BrowserManager
{
    public static string LastURL;
    public static IWebElement CurrentWebElement;
    private static IWebDriver _driver;

    private static int _playBtnX = 800;
    private static int _playBtnY = 800;

    public static InputSimulator InputSimulator;
    public static IKeyboardSimulator Keyboard => InputSimulator.Keyboard;


    [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetCursorPos(int x, int y);

    public static async void OpenUrl(string url)
    {
        TryOpenBrowser();
        LastURL = url;
        _driver.Navigate().GoToUrl(LastURL);
        await SetPlayBtnPosition();
        PlayPause();
    }


    public static void PlayPause()
    {
        SetCursorPos(_playBtnX, _playBtnY);
        InputSimulator.Mouse.LeftButtonClick();
    }

    /*private static void Play()
    {
        CurrentWebElement = _driver.FindElement(By.CssSelector(
            "#my-player > div.vjs-control-bar > button.vjs-play-control.vjs-control.vjs-button.vjs-paused"));
        CurrentWebElement?.Click();
        Keyboard.KeyPress(VirtualKeyCode.VK_F);
    }

    private static void Pause()
    {
        CurrentWebElement = _driver.FindElement(By.CssSelector(
            "#my-player > div.vjs-control-bar > button.vjs-play-control.vjs-control.vjs-button.vjs-playing"));
        CurrentWebElement?.Click();
        Keyboard.KeyPress(VirtualKeyCode.VK_F);
    }*/

    public static void ChangeQuality(string quality)
    {
        if (!TrySetElement(
                "#my-player > div.vjs-control-bar > div.vjs-quality-selector.vjs-menu-button.vjs-menu-button-popup.vjs-control.vjs-button > button"))
            return;
        TryElementClick();

        if (!TrySetElement(GetQualitySelector(quality))) return;
        TryElementClick();
    }

    private static string GetQualitySelector(string quality)
    {
        switch (quality)
        {
            case "1080":
                return
                    "#my-player > div.vjs-control-bar > div.vjs-quality-selector.vjs-menu-button.vjs-menu-button-popup.vjs-control.vjs-button > div > ul > li:nth-child(1)";
            case "720":
                return
                    "#my-player > div.vjs-control-bar > div.vjs-quality-selector.vjs-menu-button.vjs-menu-button-popup.vjs-control.vjs-button > div > ul > li:nth-child(2)";
            case "480":
                return
                    "#my-player > div.vjs-control-bar > div.vjs-quality-selector.vjs-menu-button.vjs-menu-button-popup.vjs-control.vjs-button > div > ul > li:nth-child(3)";
            case "360":
                return
                    "#my-player > div.vjs-control-bar > div.vjs-quality-selector.vjs-menu-button.vjs-menu-button-popup.vjs-control.vjs-button > div > ul > li:nth-child(4)";
        }

        return "";
    }

    public static void CloseBrowser()
    {
        _driver.Quit();
        _driver = null;
    }

    public static void Skip()
    {
        if (TrySetElement(
                "#my-player > div.vjs-overlay.vjs-overlay-bottom-left.vjs-overlay-skip-intro.vjs-overlay-background"))
            TryElementClick();

        if (TrySetElement("#my-player > div:nth-child(8)"))
            TryElementClick();
    }

    private static void TryOpenBrowser()
    {
        if (_driver != null) return;
        var options = new ChromeOptions();

        //var userName = Environment.UserName;
        //var path = $@"C:\Users\{userName}\AppData\Local\Programs\Opera\launcher.exe";

        // options.BinaryLocation = path;
        options.AddArguments("start-maximized"); // open Browser in maximized mode
        options.AddArguments("disable-infobars"); // disabling infobars
        options.AddArguments("--disable-extensions"); // disabling extensions
        options.AddArguments("--disable-gpu"); // applicable to windows os only
        options.AddArguments("--disable-dev-shm-usage"); // overcome limited resource problems
        options.AddArguments("--no-sandbox"); // Bypass OS security model
        options.AddExcludedArgument("enable-automation");
        _driver = new ChromeDriver(options);
    }

    private static bool TryElementClick()
    {
        if (CurrentWebElement.IsVisible())
        {
            CurrentWebElement.Click();
            return true;
        }

        return false;
    }

    private static async Task SetPlayBtnPosition()
    {
        await Task.Delay(3000);
        if (!TrySetElement("#my-player > button")) return;
        if (!CurrentWebElement.IsVisible()) return;

        _playBtnX = CurrentWebElement.Location.X;
        _playBtnY = CurrentWebElement.Location.Y;
    }

    private static bool TrySetElement(string selector)
    {
        try
        {
            CurrentWebElement = _driver.FindElement(By.CssSelector(selector));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static bool IsVisible(this IWebElement element)
    {
        try
        {
            return element != null && element.Displayed;
        }
        catch (Exception)
        {
            // If element is null, stale or if it cannot be located
            return false;
        }
    }
}