using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Web.Utils
{
    public class WebElement : IWebElement
    {
        public string TagName { get { return _driver.FindElement(_by).TagName; } }
        public string Text { get { return _driver.FindElement(_by).Text; } }
        public bool Enabled { get { return _driver.FindElement(_by).Enabled; } }
        public bool Selected { get { return _driver.FindElement(_by).Selected; } }
        public Point Location { get { return _driver.FindElement(_by).Location; } }
        public Size Size { get { return _driver.FindElement(_by).Size; } }
        public bool Displayed { get { return _driver.FindElement(_by).Displayed; } }

        private LykkeRemoteWebDriver _driver;
        private By _by;

        public WebElement(LykkeRemoteWebDriver driver, By by)
        {
            _driver = driver;
            _by = by;
        }

        public void Clear()
        {
            _driver.FindElement(_by).Clear();
        }

        public void Click()
        {
            WaitForElementDisplayed();
            _driver.FindElement(_by).Click();
        }

        public void ClickByJavaScript()
        {
            var jsDriver = (IJavaScriptExecutor)_driver;
            jsDriver.ExecuteScript("arguments[0].click();", _driver.FindElement(_by));
        }

        public IWebElement FindElement(By by)
        {
            return _driver.FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return _driver.FindElements(by);
        }

        public string GetAttribute(string attributeName)
        {
            return _driver.FindElement(_by).GetAttribute(attributeName);
        }

        public string GetCssValue(string propertyName)
        {
            return _driver.FindElement(_by).GetCssValue(propertyName);
        }

        public string GetProperty(string propertyName)
        {
            return _driver.FindElement(_by).GetProperty(propertyName);
        }

        public void SendKeys(string text)
        {
            _driver.FindElement(_by).SendKeys(text);
        }

        public void Submit()
        {
            _driver.FindElement(_by).Submit();
        }

        public WebElement WaitForElementPresent(int seconds = 30)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds));
            wait.IgnoreExceptionTypes(typeof(WebDriverException));
            try
            {
                wait.Until(d => d.FindElements(_by).Count > 0);
            } catch (WebDriverTimeoutException)
            {
                throw new NoSuchElementException($"Element {_by} not present on page after {seconds} seconds");
            }
            return this;
        }

        public WebElement WaitForElementDisplayed(int seconds = 30)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds));
            wait.IgnoreExceptionTypes(typeof(WebDriverException));
            try
            {
                wait.Until(d => d.FindElement(_by).Displayed);
            }
            catch (WebDriverTimeoutException)
            {
                throw new NoSuchElementException($"Element {_by} not displayed on page after {seconds} seconds");
            }
            return this;
        }

        public WebElement WaitForElementDisplayedSafe(int seconds = 30)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds));
            wait.IgnoreExceptionTypes(typeof(WebDriverException));
            try
            {
                wait.Until(d => d.FindElement(_by).Displayed);
            }
            catch (WebDriverTimeoutException)
            {
              TestContext.Progress.WriteLine($"Element {_by} not displayed on page after {seconds} seconds");
            }
            return this;
        }

        public bool IsElementPresent()
        {
            return FindElements(_by).Count > 0;
        }

        public WebElement HoverOver()
        {
            Actions builder = new Actions(_driver);
            builder.MoveToElement(_driver.FindElement(_by)).Build().Perform();
            return this;
        }
    }
}
