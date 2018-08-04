using System;
using System.Collections.Generic;
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

        #region main projet lists
        By linkProjets = By.CssSelector("main_projects_list li");

        #endregion
    }
}
