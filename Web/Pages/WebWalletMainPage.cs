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
    public class WebWalletMainPage : BasePage
    {
        public WebWalletMainPage(LykkeRemoteWebDriver driver) : base(driver)
        {
        }

        #region header objects

        By btnMenu = By.CssSelector(".header .btn_menu");
        By btnLogo = By.CssSelector(".header .header_logo");
        By btnLogin = By.CssSelector(".header .header_login");
        By lblUsername = By.CssSelector(".header_login__title");
        By btnUserIcon = By.CssSelector(".header_user__img");
        By btnSignOut = By.XPath("//a[text()='Sign out']");
        #endregion

        #region header navigation container

        By btnWallets = By.XPath("//a[text()='Wallets']");
        By btnTransfer = By.CssSelector("a[href='/transfer']");
        By lblCurrentBalance = By.CssSelector(".header_nav_balance__value span");
        By lblBaseAsset = By.CssSelector("#baseAsset");

        #endregion

        #region wallet tabs

        By btnTradingWallets = By.CssSelector("//a[text()='Trading']");
        By btnApiWallets = By.CssSelector("a[href='/wallets/hft']");

        #endregion

        By lblTradingWalletTotalBalance = By.CssSelector(".wallet__total-balance-value span > span");

        #region wallet action bar

        By btnDeposit = By.XPath("//a[text()='Deposit']");
        By btnWalletActionsTransfer = By.XPath("//*[contains(@class,'wallet-action-bar')]//a[text()='Transfer']");
        By btnHistory = By.XPath("//a[text()='History']");
        #endregion

        #region wallet menu 

        By btnWalletMenuCreditCard = By.XPath("//*[contains(@class,'wallet-menu')]//a[text()='Credit Card']");
        By btnWalletMenuBlockchainTransfer = By.XPath("//*[contains(@class,'wallet-menu')]//a[text()='Blockchain Transfer']");
        By btnWalletMenuSwift = By.XPath("//*[contains(@class,'wallet-menu')]//a[text()='SWIFT']");

        //add here wallet menu pop-ups
        #endregion

        #region currencies

        By tableCurrencies = By.XPath("//h3[text()='Currencies']//../table//tbody");
        By tableCrypto = By.XPath("//h3[text()='Crypto']//../table//tbody");
        #endregion

        #region Api wallets

        By btnNewWallet = By.XPath("//*[@class='wallet-tabs']//*[contains(text(),'New Wallet')]");
        By wallets = By.CssSelector(".wallet_list .wallet");

        #endregion

        #region side menu

        By btnSideMenuLykkeLogo = By.CssSelector(".sidebar_menu .header_logo");
        By overlay = By.CssSelector(".app.app--overlayed");
        #endregion

        #region main projet lists
        By linkProjets = By.CssSelector("main_projects_list li");

        #endregion


        public WebWalletMainPage WaitForLoaded()
        {
            WebElement(lblTradingWalletTotalBalance).WaitForElementDisplayed(60);
            return this;
        }

        public bool IsHumbergerMenuPresent() => WebElement(btnMenu).WaitForElementDisplayedSafe().IsElementPresent();
        public bool IsLykkeWalletLogoPresent() => WebElement(btnLogo).WaitForElementDisplayedSafe().IsElementPresent();
        public bool IsUserIconPresent() => WebElement(btnUserIcon).WaitForElementDisplayedSafe().IsElementPresent();
        public bool IsSignOutDisplayed() => WebElement(btnSignOut).WaitForElementDisplayedSafe().Displayed;
        public bool IsSideMenuOpened(int wait = 30) => WebElement(btnSideMenuLykkeLogo).WaitForElementDisplayedSafe(wait).Displayed;

        public string GetUserName() => WebElement(lblUsername).WaitForElementDisplayed().Text;

        public WebWalletMainPage ClickOnWebWalletIcon()
        {
            WebElement(btnLogo).Click();
            return this;
        }

        public WebWalletMainPage HoverOverUserName()
        {
            WebElement(lblUsername).HoverOver();
            return this;
        }

        public WebWalletMainPage HoverOverUserIcon()
        {
            WebElement(btnUserIcon).HoverOver();
            return this;
        }

        public WebWalletMainPage OpenAsideMenuByHumburgerButton()
        {
            if (!IsSideMenuOpened(5))
                WebElement(btnMenu).Click();
            return this;
        }

        public WebWalletMainPage CloseAsideMenuByHumburgerButton()
        {
            if (IsSideMenuOpened(5))
                WebElement(overlay).Click();
            return this;
        }

        public string GetAsideMenuLinkText(AsideMenu menu)
        {
            return WebElement(By.XPath($"//div[@class='main_projects_list__title']//*[contains(text(), '{menu.GetDescription()}')]/..")).WaitForElementDisplayed().Text;
        }

        public void ClickOnAsideMenu(AsideMenu menu)
        {
            WebElement(By.XPath($"//div[@class='main_projects_list__title']//*[contains(text(), '{menu.GetDescription()}')]/..")).Click();
            _driver.SwitchTo().Window(_driver.WindowHandles.ToList().Last());
        }

        public WebElement GetSocialElementFromMenu(SocialElementsAsideMenu menu)
        {
          return WebElement(By.XPath($"//*[@class='social']//li//span[contains(text(), '{menu.GetDescription()}')]//../../a"));
        }

        public string GetSocialElementLinkFromAsideMenu(SocialElementsAsideMenu menu)
        {
            return GetSocialElementFromMenu(menu).WaitForElementDisplayed().GetAttribute("href");
        }
    }

    public enum AsideMenu
    {
        [Description("Wallet")] LykkeWallet,
        [Description("Streams")] LykkeStreams,
        [Description("Explorer")] BlockchainExplorer
    }

    public enum SocialElementsAsideMenu
    {
        [Description("Facebook")] Facebook,
        [Description("Linkedin")] LinkedIn,
        [Description("Instagram")] Instagram,
        [Description("Twitter")] Twitter,
        [Description("Youtube")] Youtube,
        [Description("Reddit")] Reddit,
        [Description("Telegram")] Telegram,
    }

    public static class EnumExtension
    {
        public static string GetDescription(this Enum t)
        {
            FieldInfo fi = t.GetType().GetField(t.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return t.ToString();
        }
    }
}
