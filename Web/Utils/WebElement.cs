using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Web.Utils
{
    public class WebElement : IWebElement
    {
        public string TagName { get; }
        public string Text { get; }
        public bool Enabled { get; }
        public bool Selected { get; }
        public Point Location { get; }
        public Size Size { get; }
        public bool Displayed { get; }

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
            _driver.FindElement(_by).Click();
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
            wait.Until(d => d.FindElements(_by).Count > 0);

            return this;
        }

        public WebElement WaitForElementDisplayed(int seconds = 30)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds));
            wait.IgnoreExceptionTypes(typeof(WebDriverException));
            wait.Until(d => d.FindElement(_by).Displayed);

            return this;
        }

        public bool IsElementPresent()
        {
            return FindElements(_by).Count > 0;
        }
    }
}
