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
                Step("Открываем страницу логина", () => 
                {
                    Driver1 = CreateWebDriver();
                    Driver1.Navigate().GoToUrl("https://auth-dev.lykkex.net/signin");
                });

                Step("Вводим креды", () => 
                {
                    new LWLoginPage(Driver1).LogIn("lykke_autotest_a0d3f2ef62@lykke.com", "f132ce9b72d812a6ae99f08aa82ee0dff7f535c82363e793b5c9dd634900d10f");
                });

                Step("Кликаем на капчу", () => 
                {
                    new LWLoginPage(Driver1).ClickRecaptchCheckBox();
                });
            }
        }

        public class GoogleExample : WebBaseTest
        {
            [Test]
            [Category("WebWallet")]
            public void GoogleExampleTest()
            {
                var link = "https://www.google.com";
                Step($"Открываем страницу {link}", () =>
                {
                    Driver1 = CreateWebDriver();
                    Driver1.Navigate().GoToUrl(link);
                });

                Step("В поисковую строку вводим Lykke", () =>
                {
                    new LWLoginPage(Driver1).SearchGoogleFor("lykke");
                });

                Step("Открываем перый результат поиска", () =>
                {
                    new LWLoginPage(Driver1).ClickOnFirstLink();
                });

                Step("Проверяем, что загрузилась страницу Lykke.com", () => 
                {
                    Assert.That(Driver1.Url, Does.Contain("lykke.com"));
                });
            }
        }
    }
}
