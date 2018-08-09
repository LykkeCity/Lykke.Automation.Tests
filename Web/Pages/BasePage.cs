using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using OpenQA.Selenium;
using Web.Utils;

namespace Web.Pages
{
    public class BasePage
    {
        protected LykkeRemoteWebDriver _driver;

        public BasePage(LykkeRemoteWebDriver driver)
        {
            _driver = driver;
        }

        protected WebElement WebElement(By by)
        {
            return new WebElement(_driver, by);
        }

        public void SwitchToDefaultWindow()
        {
            _driver.SwitchTo().Window(_driver.WindowHandles.ToList().First());
        }
    }
}
