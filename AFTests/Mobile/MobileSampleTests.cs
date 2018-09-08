using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobile.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Remote;
using XUnitTestCommon.TestsData;

namespace AFTests.Mobile
{
    public class MobileSampleTests
    {      
        public class FirstTets : MobileBaseTest
        {
            [Test]
            [Category("Mobile")]
            public void AndroidTest()
            {
                string email = TestData.GenerateEmail();
                string phoneNumber = TestData.GenerateNumbers(9);

                Step("Устанавливаем PIN на телефон", () => { new LogInPage(Driver).SetPin(); });

                Step("Выбираем Dev env server", () => { new LogInPage(Driver).SetDevEnv(); });

                Step("Жмем Регистрацию", () => { new LogInPage(Driver).ClickRegisterButton(); });

                Step("Вводим email", () => { new LogInPage(Driver).SetEmail(email); });

                Step("Вводим code", () => { new LogInPage(Driver).SetCode(); });

                Step("Задаем пароль", () => { new LogInPage(Driver).SetPassword(); });

                Step("Вводим имя и телефон", () => { new LogInPage(Driver).SetUserInfo(phoneNumber: phoneNumber); });

                Step("Подтверждаем код с смс", () => { new LogInPage(Driver).SetCode(); });

                Step("Устанавливаем пин транзакций", () => { new LogInPage(Driver).SetTransactionPin(); });

                Step("Свайпаем для геренации ключа", () => { new LogInPage(Driver).SwipeToGenerateKey(); });

                Step("Отказываемся от генерации бэкапа", () => { new LogInPage(Driver).CancelBackUp(); });

                Step("Проверяем, что загрузилась главная страница приложения", () => { new LogInPage(Driver).IsActionBarVisible(); });
            }
        }
    }
}
