using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Remote;

namespace Mobile.Pages
{
    public class LogInPage : MobileBasePage
    {

        public LogInPage(AndroidDriver<AppiumWebElement> driver) : base(driver)
        {
        }

        By btnChangeServer = By.Id("btnChangeServer"); 
        By btnRadioDev = By.Id("radioDev");
        By btnSave = By.Id("saveBtn");

        #region set pin
        By btnCancelPin = By.Id("android:id/button2");
        By btnOkPin = By.Id("android:id/button1");
        By btnContinueWithoutFingerPrint = By.Id("com.android.settings:id/lock_none");
        By btnPin = By.Id("com.android.settings:id/lock_pin");
        By btnNoPinWhenBoot = By.Id("com.android.settings:id/encrypt_dont_require_password");

        By txtSetPin = By.Id("com.android.settings:id/password_entry");
        By btnNextPin = By.Id("com.android.settings:id/next_button");

        By btnShowAllNotifications = By.Id("com.android.settings:id/show_all");
        By btnDoneNotifications = By.Id("com.android.settings:id/redaction_done_button");
        #endregion

        #region registration

        By btnRegister = By.Id("com.lykkex.LykkeWallet:id/registerButton");

        By txtEmail = By.Id("com.lykkex.LykkeWallet:id/emailEditText");
        By btnConfirmEmail = By.Id("com.lykkex.LykkeWallet:id/confirmEmailButton");

        By txtCode1 = By.Id("com.lykkex.LykkeWallet:id/codeEditText");
        By btnNext = By.Id("com.lykkex.LykkeWallet:id/tvNext");

        By txtPassword = By.Id("com.lykkex.LykkeWallet:id/passwordEditText");
        By txtPasswordConfirm = By.Id("com.lykkex.LykkeWallet:id/confirmPasswordEditText");
        By txtSecretWord = By.Id("com.lykkex.LykkeWallet:id/hintPassword");

        By txtFullName = By.Id("com.lykkex.LykkeWallet:id/fullNameEditText");
        By txtPhone = By.Id("com.lykkex.LykkeWallet:id/editPhone");

        By txtCodePhone = By.Id("com.lykkex.LykkeWallet:id/codeEditText");
        By btnOneButton = By.Id("com.lykkex.LykkeWallet:id/tvOne1");

        By cmpDotImagesToSwipe = By.Id("com.lykkex.LykkeWallet:id/dotsImageView");

        By btnCancelBackUp = By.Id("com.lykkex.LykkeWallet:id/closeImageView");

        // same verify pin

        #endregion

        #region action bar
        By cmpActionBar = By.Id("com.lykkex.LykkeWallet:id/action_bar");

        #endregion

        public LogInPage ClickRegisterButton()
        {
            WaitForElementDisplayed(btnRegister).Click();
            return this;
        }

        public LogInPage SetEmail(string email)
        {
            WaitForElementDisplayed(txtEmail).SendKeys(email);
            WaitForElementDisplayed(btnConfirmEmail).Click();
            return this;
        }

        public LogInPage SetCode(string code = "0000")
        {
            WaitForElementDisplayed(txtCode1).SendKeys(code);
            WaitForElementDisplayed(btnNext).Click();
            return this;
        }

        public LogInPage SetPassword(string password = "111111", string hint = "lykke")
        {
            WaitForElementDisplayed(txtPassword).SendKeys(password);
            WaitForElementDisplayed(txtPasswordConfirm).SendKeys(password);
            WaitForElementDisplayed(txtSecretWord).SendKeys(hint);
            WaitForElementDisplayed(btnNext).Click();

            return this;
        }

        public LogInPage SetUserInfo(string fullName = "Auto test", string phoneNumber = "1234567")
        {
            WaitForElementDisplayed(txtFullName).SendKeys(fullName);
            WaitForElementDisplayed(txtPhone).SendKeys(phoneNumber);
            WaitForElementDisplayed(btnNext).Click();
            WaitForElementDisplayed(btnOkPin).Click();
            WaitForElementDisplayed(btnNext).Click();

            return this;
        }

        public LogInPage SetDevEnv()
        {
            WaitForElementDisplayed(btnChangeServer).Click();
            WaitForElementDisplayed(btnRadioDev).Click();
            WaitForElementDisplayed(btnSave).Click();

            return this;
        }

        public LogInPage SetTransactionPin()
        {
            WaitForElementDisplayed(btnOneButton).Click();
            WaitForElementDisplayed(btnOneButton).Click();
            WaitForElementDisplayed(btnOneButton).Click();
            WaitForElementDisplayed(btnOneButton).Click();

            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

            WaitForElementDisplayed(btnOneButton).Click();
            WaitForElementDisplayed(btnOneButton).Click();
            WaitForElementDisplayed(btnOneButton).Click();
            WaitForElementDisplayed(btnOneButton).Click();
            return this;
        }

        public LogInPage SwipeToGenerateKey()
        {
            int count = 15;

            while (count-- > 0 && !IsElementDisplayed(btnCancelBackUp))
            {
                var tAction = new TouchAction(_driver);
                var cmp = WaitForElementDisplayed(cmpDotImagesToSwipe);
                double x = cmp.Location.X;
                double y = cmp.Location.Y;
                var middleHeight = cmp.Size.Height / 2;
                var middleWidth = cmp.Size.Width / 2;

                var xLeft = x;
                var xRight = cmp.Size.Width - x;

                var yMid = y + middleHeight;

                tAction.Press(x + 6 * count, y + 6 * count).Wait(1500).MoveTo(cmp).Release().Perform();
            }
            return this;
        }

        public LogInPage CancelBackUp()
        {
            WaitForElementDisplayed(btnCancelBackUp).Click();
            return this;
        }

        public bool IsActionBarVisible()
        {
            return IsElementDisplayed(cmpActionBar, 15);
        }

        public LogInPage ClickCancelOnSetUpPin()
        {
            WaitForElementDisplayed(btnCancelPin).Click();
            return this;
        }

        public LogInPage SetPin()
        {
            WaitForElementDisplayed(btnOkPin).Click();
            WaitForElementDisplayed(btnContinueWithoutFingerPrint).Click();
            WaitForElementDisplayed(btnPin).Click();
            WaitForElementDisplayed(btnNoPinWhenBoot).Click();
            WaitForElementDisplayed(txtSetPin).SendKeys("1111");
            WaitForElementDisplayed(btnNextPin).Click();
            WaitForElementDisplayed(txtSetPin).SendKeys("1111");
            WaitForElementDisplayed(btnNextPin).Click();
            WaitForElementDisplayed(btnShowAllNotifications).Click();
            WaitForElementDisplayed(btnDoneNotifications).Click();

            return this;
        }

        public void SetPinViaShell()
        {
            System.Diagnostics.Process.Start("adb", "shell locksettings set-pin 1111");
        }
    }
}
