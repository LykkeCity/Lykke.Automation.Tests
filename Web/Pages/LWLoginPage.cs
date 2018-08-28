using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using Web.Utils;

namespace Web.Pages
{
    public class LWLoginPage : BasePage
    {
        public LWLoginPage(LykkeRemoteWebDriver driver) : base(driver)
        {
            this._driver = driver;
        }

        private By txtEmail = By.CssSelector("input#login-email");
        private By txtPass = By.CssSelector("input#Password");
        private By btnSignIn = By.CssSelector("div.submit-group button");
        private By chkRecaptcha = By.CssSelector("div.login-recaptcha");

        public void LogIn(string email, string password)
        {
            WebElement(txtEmail).WaitForElementPresent().Clear();
            WebElement(txtEmail).SendKeys(email);
            WebElement(txtPass).WaitForElementDisplayed().Clear();
            WebElement(txtPass).SendKeys(password);
            WebElement(btnSignIn).Click();
            ClickRecaptchCheckBox();
            WebElement(btnSignIn).Click();
        }

        public void ClickRecaptchCheckBox()
        {
            var source0 = _driver.PageSource;
            _driver.SwitchTo().Frame(_driver.FindElement(By.CssSelector("iframe")));
            var source = _driver.PageSource;
            var el = WebElement(By.CssSelector("#recaptcha-anchor .recaptcha-checkbox-checkmark"));
            var present = el.IsElementPresent();
            el.WaitForElementPresent().ClickByJavaScript();

            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
            _driver.SwitchTo().DefaultContent();
        }


        #region google area
        By txtGoogleSearchInput = By.CssSelector("form.tsf input#lst-ib");
        By linkFirstSearchResult = By.CssSelector("#search h3 a");
        By select = By.CssSelector("ul[role='listbox'] li");

        public LWLoginPage SearchGoogleFor(string searchQuerry)
        {
            WebElement(txtGoogleSearchInput).WaitForElementDisplayed().SendKeys(searchQuerry);
            Actions act = new Actions(_driver);
            act.SendKeys(Keys.Enter);
            return this;
        }

        public void ClickOnFirstLink()
        {
            //
            WebElement(select).FindElements(By.CssSelector("div")).ToList().Find(l => l.Text == "lykke").Click();
            WebElement(linkFirstSearchResult).WaitForElementDisplayed().Click();
        }
        #endregion
    }
}
