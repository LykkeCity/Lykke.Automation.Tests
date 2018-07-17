using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Remote;

namespace Mobile.Pages
{
    public class MobileBasePage
    {
        protected AndroidDriver<AppiumWebElement> _driver;

        public MobileBasePage (AndroidDriver<AppiumWebElement> driver)
        {
            _driver = driver;
        }

        public AppiumWebElement WaitForElementDisplayed(AppiumWebElement element, int time = 30)
        {
            while(time-- > 0)
            {
                try
                {
                    if (element.Displayed)
                        return element;
                }
                catch
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
            throw new ElementNotVisibleException($"Element {element.Id} is not visible after {time} seconds");
        }

        public AppiumWebElement WaitForElementDisplayed(By element, int time = 30)
        {
            while (time-- > 0)
            {
                try
                {
                    var _element = _driver.FindElement(element);
                    if (_element.Displayed)
                        return _element;
                }
                catch
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
            throw new ElementNotVisibleException($"Element {element} is not visible after {time} seconds");
        }

        public bool IsElementDisplayed(By element, int time = 2)
        {
            while (time-- > 0)
            {
                try
                {
                    var _element = _driver.FindElement(element);
                    return _element.Displayed;
                }
                catch
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
            return false;
        }

        //protected static AppiumWebElement AppiumWebElement(By by)
        //{
        //    return _driver.FindElement(by);
        //}
    }
}
