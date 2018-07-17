using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Remote;

namespace Web.Utils
{
    public class LykkeRemoteWebDriver : IWebDriver, ITakesScreenshot, IAllowsFileDetection, IHasSessionId,
                                        IJavaScriptExecutor, IActionExecutor, IHasInputDevices
    {
        public string Url { get { return _driver.Url; } set { _driver.Url = value; } }
        public string Title { get { return _driver.Title; } }
        public string PageSource { get { return _driver.PageSource; } }
        public string CurrentWindowHandle { get; }
        public ReadOnlyCollection<string> WindowHandles { get { return _driver.WindowHandles; } }
        public string HubUrl { get;  private set; }

        private RemoteWebDriver _driver;

        public LykkeRemoteWebDriver(string url = "http://127.0.0.1:4444/wd/hub")
        {
            HubUrl = url;
            var options = new ChromeOptions() { };
            options.AddArguments("start-maximized");
            var caps = options.ToCapabilities() as DesiredCapabilities;
            caps.SetCapability("enableVideo", true);

            _driver = new RemoteWebDriver(new Uri(HubUrl), caps);
        }

        public void Close()
        {
            _driver.Close();
        }

        public void Dispose()
        {
            _driver.Dispose();
        }

        public IWebElement FindElement(By by)
        {
            return _driver.FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return _driver.FindElements(by);
        }

        public IOptions Manage()
        {
            return _driver.Manage();
        }

        public INavigation Navigate()
        {
            return _driver.Navigate();
        }

        public void Quit()
        {
            _driver.Quit();
        }

        public ITargetLocator SwitchTo()
        {
            return _driver.SwitchTo();
        }

        public Screenshot GetScreenshot()
        {
            return _driver.GetScreenshot();
        }

        public object ExecuteScript(string script, params object[] args)
        {
            return _driver.ExecuteScript(script, args);
        }

        public object ExecuteAsyncScript(string script, params object[] args)
        {
            return _driver.ExecuteAsyncScript(script, args);
        }

        public void PerformActions(IList<ActionSequence> actionSequenceList)
        {
            _driver.PerformActions(actionSequenceList);
        }

        public void ResetInputState()
        {
            _driver.ResetInputState();
        }

        public SessionId SessionId => _driver.SessionId;

        public IFileDetector FileDetector
        { get => _driver.FileDetector; set => _driver.FileDetector = value; }

        public bool IsActionExecutor => ((IActionExecutor)_driver).IsActionExecutor;

        public IKeyboard Keyboard => ((IHasInputDevices)_driver).Keyboard;

        public IMouse Mouse => ((IHasInputDevices)_driver).Mouse;
    }
}
