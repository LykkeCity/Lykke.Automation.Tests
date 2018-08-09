using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Web.Pages;
using Web.Utils;

namespace AFTests.Web
{
    public class WebWalletTests
    {
        public class WebWalletLogIn : WebBaseTest
        {
            [Test]
            [Category("WebWallet")]
            public void WebWalletLogInTest()
            {
                Step($"Open {WebWalletUrl}", () => 
                {
                    Driver1 = CreateWebDriver();
                    Driver1.Navigate().GoToUrl(WebWalletUrl);
                });

                Step("Log In as user", () => 
                {
                    new LWLoginPage(Driver1).LogIn("lykke_autotest_a0d3f2ef62@lykke.com", "f132ce9b72d812a6ae99f08aa82ee0dff7f535c82363e793b5c9dd634900d10f");
                });

                var webWalletPage = new WebWalletMainPage(Driver1);

                Step("Verify that Menu, User Name, User Icon and Lykke logo are displayed", () => 
                {
                    webWalletPage.WaitForLoaded();
                    Assert.Multiple(() => 
                    {
                        Assert.That(webWalletPage.IsHumbergerMenuPresent(), Is.True, "Humburger menu not present");
                        Assert.That(webWalletPage.IsLykkeWalletLogoPresent(), Is.True, "Lykke logo not present");
                        Assert.That(webWalletPage.IsUserIconPresent(), Is.True, "User Icon not present");
                        Assert.That(webWalletPage.GetUserName(), Is.Not.Null.Or.Empty, "User name is null or empty");
                    });
                });

                Step("Hover over User Icon and verify that Sign out displayed", () => 
                {
                    webWalletPage.HoverOverUserIcon();
                    Assert.That(webWalletPage.IsSignOutDisplayed(), Is.True, "Sign out not displayed");
                });

                Step("Hover over User Name and verify that Sign out displayed", () =>
                {
                    webWalletPage.HoverOverUserName();
                    Assert.That(webWalletPage.IsSignOutDisplayed(), Is.True, "Sign out not displayed");
                });

                Step("Click on Lykke logo and check, that lykke.com page will be opened. Go back in case of success", () => 
                {
                    webWalletPage.ClickOnWebWalletIcon();
                    Assert.That(() => Driver1.Url, Does.Contain("webwallet-dev.lykkex.net").After(30).Seconds.PollEvery(1).Seconds, $"URL does not contain {WebWalletUrl}");

                    Driver1.Navigate().Back();
                    webWalletPage.WaitForLoaded();
                });

                Step("Click on humburger-menu and validate aside links to Lykke wallet, Lykke Stream and Blockchain Explorer", () => 
                {
                    webWalletPage.OpenAsideMenuByHumburgerButton();
                    Assert.That(() => webWalletPage.GetAsideMenuLinkText(AsideMenu.BlockchainExplorer), Is.Not.Null.Or.Empty.After(20).Seconds.PollEvery(1).Seconds, "Blockchain explorer link not visible");
                    Assert.That(() => webWalletPage.GetAsideMenuLinkText(AsideMenu.LykkeStreams), Is.Not.Null.Or.Empty.After(20).Seconds.PollEvery(1).Seconds, "Lykke Streams link not visible");
                    Assert.That(() => webWalletPage.GetAsideMenuLinkText(AsideMenu.LykkeWallet), Is.Not.Null.Or.Empty.After(20).Seconds.PollEvery(1).Seconds, "Lykke wallet link not visible");
                });

                Step("Click on Lykke wallet link and verify that it link to Lykke wallet page", () => 
                {
                    webWalletPage.ClickOnAsideMenu(AsideMenu.LykkeWallet);
                    Assert.That(() => Driver1.Url, Does.Contain("lykke.com").After(30).Seconds.PollEvery(1).Seconds, "URL does not contain lykke.com");
                    webWalletPage.SwitchToDefaultWindow();
                });

                Step("Click on Lykke Streams link and verify that it link to Lykke streams page", () =>
                {
                    webWalletPage.OpenAsideMenuByHumburgerButton();
                    webWalletPage.ClickOnAsideMenu(AsideMenu.LykkeStreams);
                    Assert.That(() => Driver1.Url, Does.Contain("streams.lykke.com").After(30).Seconds.PollEvery(1).Seconds, "URL does not contain https://streams.lykke.com/");
                    webWalletPage.SwitchToDefaultWindow();
                });

                Step("Click on Lykke Blockchain Explorer link and verify it links to Blockchain Explorer", () =>
                {
                    webWalletPage.OpenAsideMenuByHumburgerButton();
                    webWalletPage.ClickOnAsideMenu(AsideMenu.BlockchainExplorer);
                    Assert.That(() => Driver1.Url, Does.Contain("blockchainexplorer.lykke.com").After(30).Seconds.PollEvery(1).Seconds, "URL does not contain https://blockchainexplorer.lykke.com/");
                    webWalletPage.SwitchToDefaultWindow();
                });

                Step("Verify facebook link", () => 
                {
                    webWalletPage.OpenAsideMenuByHumburgerButton();
                    Assert.That(webWalletPage.GetSocialElementLinkFromAsideMenu(SocialElementsAsideMenu.Facebook),
                        Does.Contain("https://www.facebook.com/groups/542506412568917"), "Unexpected social facebook link");
                });

                Step("Verify Instagram link", () =>
                {
                    webWalletPage.OpenAsideMenuByHumburgerButton();
                    Assert.That(webWalletPage.GetSocialElementLinkFromAsideMenu(SocialElementsAsideMenu.Instagram),
                        Does.Contain("http://instagram.com/lykkecity"), "Unexpected social Instagram link");
                });

                Step("Verify Twitter link", () =>
                {
                    webWalletPage.OpenAsideMenuByHumburgerButton();
                    Assert.That(webWalletPage.GetSocialElementLinkFromAsideMenu(SocialElementsAsideMenu.Twitter),
                        Does.Contain("https://twitter.com/lykke"), "Unexpected social Twitter link");
                });

                Step("Verify Youtube link", () =>
                {
                    webWalletPage.OpenAsideMenuByHumburgerButton();
                    Assert.That(webWalletPage.GetSocialElementLinkFromAsideMenu(SocialElementsAsideMenu.Youtube),
                        Does.Contain("https://www.youtube.com/channel/UCmMYipGdKMF0kzfaE-PXsNQ"), "Unexpected social Youtube link");
                });

                Step("Verify LinkedIn link", () =>
                {
                    webWalletPage.OpenAsideMenuByHumburgerButton();
                    Assert.That(webWalletPage.GetSocialElementLinkFromAsideMenu(SocialElementsAsideMenu.LinkedIn),
                        Does.Contain("https://www.linkedin.com/company/lykke"), "Unexpected social LinkedIn link");
                });

                Step("Verify Reddit link", () =>
                {
                    webWalletPage.OpenAsideMenuByHumburgerButton();
                    Assert.That(webWalletPage.GetSocialElementLinkFromAsideMenu(SocialElementsAsideMenu.Reddit),
                        Does.Contain("https://www.reddit.com/r/lykke"), "Unexpected social Reddit link");
                });

                Step("Verify Telegram link", () =>
                {
                    webWalletPage.OpenAsideMenuByHumburgerButton();
                    Assert.That(webWalletPage.GetSocialElementLinkFromAsideMenu(SocialElementsAsideMenu.Telegram),
                        Does.Contain("https://t.co/TmjMYnQD7T"), "Unexpected social Telegram link");
                });
            }
        }
    }
}
