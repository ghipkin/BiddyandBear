using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using BB.Implementation;
using BB.Implementation.Config;
using BB.DataLayer;
using BB.DataContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BB.Implementation.UnitTest
{

    [TestClass]
    public class SecurityBehaviour
    {
        const string GOOD_PASSWORD = "XXtt__88";
        const string SHORT_PASSWORD = "Xt_9";
        const string PASSWORD_ALL_CAPS = "XXTT__88";
        const string PASSWORD_ALL_LOWER = "xxtt__88";
        const string PASSWORD_ALL_NUM = "11223344";
        const string PASSWORD_ALL_SYMBOL = "!!££$$%%";
        const string PASSWORD_NO_SYMBOL = "XXttss88";
        const string PASSWORD_NO_NUMBER = "XXttss__";

        const string PREV_PWD_SALT = "12345678";
        const string PASSWORD1 = "password1";
        const string PASSWORD2 = "password2";
        const string PASSWORD3 = "password3";
        const string PASSWORD4 = "password4";
        const string PASSWORD5 = "password5";
        const string PASSWORD6 = "password6";

        static SecuritySection SecuritySettings;

        static SecurityMethods security;

        [ClassInitialize]
        public static void Initialise(TestContext testcontext)
        {
            SecuritySettings = (SecuritySection)ConfigurationManager.GetSection("passwordPolicies");
            security = new SecurityMethods();
        }

        [TestInitialize]
        public void Defaultsettings()
        {
            SecuritySettings.PasswordPolicies["Length"].value = "0";
            SecuritySettings.PasswordPolicies["SymbolChars"].value = "0";
            SecuritySettings.PasswordPolicies["UppercaseChars"].value = "0";
            SecuritySettings.PasswordPolicies["LowercaseChars"].value = "0";
            SecuritySettings.PasswordPolicies["NumberChars"].value = "0";
            SecuritySettings.PasswordPolicies["PreviousPwdsToCheck"].value = "0";
        }

        [TestMethod]
        public void PasswordTooShort()
        {
            //ARRANGE
            var PasswordLengthpolicy = SecuritySettings.PasswordPolicies["Length"];
            PasswordLengthpolicy.value = "8";

            //ACT
            var response = security.CheckPassword(SHORT_PASSWORD, null);

            //ASSERT
            Assert.AreEqual(false, response.PasswordOK);
            Assert.AreEqual(PasswordLengthpolicy.FormattedMessage, response.Message);
        }
        [TestMethod]
        public void PasswordShortbutNottooShort()
        {
            //ARRANGE
            var PasswordLengthpolicy = SecuritySettings.PasswordPolicies["Length"];
            PasswordLengthpolicy.value = "4";

            //ACT
            var response = security.CheckPassword(SHORT_PASSWORD, null);

            //ASSERT
            Assert.IsTrue(response.PasswordOK);
            Assert.IsTrue(String.IsNullOrEmpty(response.Message));
        }
        [TestMethod]
        public void PasswordLengthNotRetireved()
        {
            //ARRANGE
            var PasswordLengthpolicy = SecuritySettings.PasswordPolicies["Length"];
            PasswordLengthpolicy.value = "w";

            //ACT
            string ErrorMessage = string.Empty;
            bool ErrorOccurred = false;
            try
            {
                var response = security.CheckPassword(SHORT_PASSWORD, null);
            }
            catch(Exception e)
            {
                ErrorOccurred = true;
                ErrorMessage = e.Message;
            }

            //ASSERT
            Assert.IsTrue(ErrorOccurred);
            Assert.AreEqual(SecurityMethods.PASSWORD_MIN_LENGTH_MISSING, ErrorMessage);
        }

        [TestMethod]
        public void PasswordHasNoSymbols()
        {
            //ARRANGE
            var PasswordSymbolpolicy = SecuritySettings.PasswordPolicies["SymbolChars"];
            PasswordSymbolpolicy.value = "2";

            //ACT
            var response = security.CheckPassword(PASSWORD_NO_SYMBOL, null);

            //ASSERT
            Assert.AreEqual(false, response.PasswordOK);
            Assert.AreEqual(PasswordSymbolpolicy.FormattedMessage, response.Message);
        }

        [TestMethod]
        public void PasswordHasNoSymbolsButAllowed()
        {
            //ARRANGE
            var PasswordSymbolpolicy = SecuritySettings.PasswordPolicies["SymbolChars"];
            PasswordSymbolpolicy.value = "0";

            //ACT
            var response = security.CheckPassword(PASSWORD_NO_SYMBOL, null);

            //ASSERT
            Assert.IsTrue(response.PasswordOK);
            Assert.IsTrue(String.IsNullOrEmpty(response.Message));
        }

        [TestMethod]
        public void PasswordNoOfSymbolsNotRetrieved()
        {
            //ARRANGE
            var PasswordSymbolpolicy = SecuritySettings.PasswordPolicies["SymbolChars"];
            PasswordSymbolpolicy.value = "X";

            //ACT
            string ErrorMessage = string.Empty;
            bool ErrorOccurred = false;
            try
            {
                var response = security.CheckPassword(PASSWORD_NO_SYMBOL, null);
            }
            catch (Exception e)
            {
                ErrorOccurred = true;
                ErrorMessage = e.Message;
            }

            //ASSERT
            Assert.IsTrue(ErrorOccurred);
            Assert.AreEqual(SecurityMethods.PASSWORD_MIN_SYMBOLS_MISSING, ErrorMessage);
        }

        [TestMethod]
        public void PasswordHasNoNumbers()
        {
            //ARRANGE
            var PasswordNumberPolicy = SecuritySettings.PasswordPolicies["NumberChars"];
            PasswordNumberPolicy.value = "2";

            //ACT
            var response = security.CheckPassword(PASSWORD_NO_NUMBER, null);

            //ASSERT
            Assert.AreEqual(false, response.PasswordOK);
            Assert.AreEqual(PasswordNumberPolicy.FormattedMessage, response.Message);
        }

        [TestMethod]
        public void PasswordHasNoNumbersButAllowed()
        {
            //ARRANGE
            SecuritySettings.PasswordPolicies["NumberChars"].value = "0";

            //ACT
            var response = security.CheckPassword(PASSWORD_NO_NUMBER, null);

            //ASSERT
            Assert.IsTrue(response.PasswordOK);
            Assert.IsTrue(String.IsNullOrEmpty(response.Message));
        }

        [TestMethod]
        public void PasswordNoOfNumbersNotRetrieved()
        {
            //ARRANGE
            SecuritySettings.PasswordPolicies["NumberChars"].value = "X";

            //ACT
            string ErrorMessage = string.Empty;
            bool ErrorOccurred = false;
            try
            {
                var response = security.CheckPassword(PASSWORD_NO_NUMBER, null);
            }
            catch (Exception e)
            {
                ErrorOccurred = true;
                ErrorMessage = e.Message;
            }

            //ASSERT
            Assert.IsTrue(ErrorOccurred);
            Assert.AreEqual(SecurityMethods.PASSWORD_MIN_NUMBER_CHARS_MISSING, ErrorMessage);
        }

        [TestMethod]
        public void PasswordHasNoLowerCase()
        {
            //ARRANGE
            var PasswordLowerCasePolicy = SecuritySettings.PasswordPolicies["LowercaseChars"];
            PasswordLowerCasePolicy.value = "2";

            //ACT
            var response = security.CheckPassword(PASSWORD_ALL_CAPS, null);

            //ASSERT
            Assert.AreEqual(false, response.PasswordOK);
            Assert.AreEqual(PasswordLowerCasePolicy.FormattedMessage, response.Message);
        }

        [TestMethod]
        public void PasswordHasNoLowerCaseButAllowed()
        {
            //ARRANGE
            SecuritySettings.PasswordPolicies["LowercaseChars"].value = "0";

            //ACT
            var response = security.CheckPassword(PASSWORD_ALL_CAPS, null);

            //ASSERT
            Assert.IsTrue(response.PasswordOK);
            Assert.IsTrue(String.IsNullOrEmpty(response.Message));
        }

        [TestMethod]
        public void PasswordNoOfLowerCaseNotRetrieved()
        {
            //ARRANGE
            SecuritySettings.PasswordPolicies["LowercaseChars"].value = "X";

            //ACT
            string ErrorMessage = string.Empty;
            bool ErrorOccurred = false;
            try
            {
                var response = security.CheckPassword(PASSWORD_ALL_CAPS, null);
            }
            catch (Exception e)
            {
                ErrorOccurred = true;
                ErrorMessage = e.Message;
            }

            //ASSERT
            Assert.IsTrue(ErrorOccurred);
            Assert.AreEqual(SecurityMethods.PASSWORD_MIN_LOWERCASE_MISSING, ErrorMessage);
        }

        [TestMethod]
        public void PasswordHasNoUpperCase()
        {
            //ARRANGE
            var PasswordUpperCasePolicy = SecuritySettings.PasswordPolicies["UppercaseChars"];
            PasswordUpperCasePolicy.value = "2";

            //ACT
            var response = security.CheckPassword(PASSWORD_ALL_LOWER, null);

            //ASSERT
            Assert.AreEqual(false, response.PasswordOK);
            Assert.AreEqual(PasswordUpperCasePolicy.FormattedMessage, response.Message);
        }

        [TestMethod]
        public void PasswordHasNoUpperCaseButAllowed()
        {
            //ARRANGE
            SecuritySettings.PasswordPolicies["UppercaseChars"].value = "0";

            //ACT
            var response = security.CheckPassword(PASSWORD_ALL_LOWER, null);

            //ASSERT
            Assert.IsTrue(response.PasswordOK);
            Assert.IsTrue(String.IsNullOrEmpty(response.Message));
        }

        [TestMethod]
        public void PasswordNoOfUpperCaseNotRetrieved()
        {
            //ARRANGE
            SecuritySettings.PasswordPolicies["UppercaseChars"].value = "X";

            //ACT
            string ErrorMessage = string.Empty;
            bool ErrorOccurred = false;
            try
            {
                var response = security.CheckPassword(PASSWORD_ALL_LOWER, null);
            }
            catch (Exception e)
            {
                ErrorOccurred = true;
                ErrorMessage = e.Message;
            }

            //ASSERT
            Assert.IsTrue(ErrorOccurred);
            Assert.AreEqual(SecurityMethods.PASSWORD_MIN_UPPERCASE_MISSING, ErrorMessage);
        }

        [TestMethod]
        public void PreviousPasswordFoundandInScope()
        {
            //ARRANGE
            var PreviousPasswordPolicy = SecuritySettings.PasswordPolicies["PreviousPwdsToCheck"];
            PreviousPasswordPolicy.value = "6";

            //ACT
            var response = security.CheckPassword(PASSWORD6, GetPasswordList());

            //ASSERT
            Assert.AreEqual(false, response.PasswordOK);
            Assert.AreEqual(PreviousPasswordPolicy.FormattedMessage, response.Message);
        }

        [TestMethod]
        public void PreviousPasswordNotInScope()
        {
            //ARRANGE
            SecuritySettings.PasswordPolicies["PreviousPwdsToCheck"].value = "0";

            //ACT
            var response = security.CheckPassword(PASSWORD_ALL_LOWER, GetPasswordList());

            //ASSERT
            Assert.IsTrue(response.PasswordOK);
            Assert.IsTrue(String.IsNullOrEmpty(response.Message));
        }

        [TestMethod]
        public void PreviousPasswordLimitNotRetrieved()
        {
            //ARRANGE
            SecuritySettings.PasswordPolicies["PreviousPwdsToCheck"].value = "X";

            //ACT
            string ErrorMessage = string.Empty;
            bool ErrorOccurred = false;
            try
            {
                var response = security.CheckPassword(PASSWORD_ALL_LOWER, null);
            }
            catch (Exception e)
            {
                ErrorOccurred = true;
                ErrorMessage = e.Message;
            }

            //ASSERT
            Assert.IsTrue(ErrorOccurred);
            Assert.AreEqual(SecurityMethods.PASSWORD_PREVIOUS_TO_CHECK_MISSING, ErrorMessage);
        }

        private List<ADL_PreviousPassword> GetPasswordList()
        {
            var Passwords = new List<ADL_PreviousPassword>();
            Passwords.Add(new DL_PreviousPassword { Salt = PREV_PWD_SALT,
                PasswordHash = security.GetPasswordHash(Encoding.ASCII.GetBytes(PREV_PWD_SALT), PASSWORD1) });
            Passwords.Add(new DL_PreviousPassword
            { Salt = PREV_PWD_SALT,
                PasswordHash = security.GetPasswordHash(Encoding.ASCII.GetBytes(PREV_PWD_SALT), PASSWORD2) });
            Passwords.Add(new DL_PreviousPassword
            { Salt = PREV_PWD_SALT,
                PasswordHash = security.GetPasswordHash(Encoding.ASCII.GetBytes(PREV_PWD_SALT), PASSWORD3) });
            Passwords.Add(new DL_PreviousPassword
            { Salt = PREV_PWD_SALT,
                PasswordHash = security.GetPasswordHash(Encoding.ASCII.GetBytes(PREV_PWD_SALT), PASSWORD4) });
            Passwords.Add(new DL_PreviousPassword
            { Salt = PREV_PWD_SALT,
                PasswordHash = security.GetPasswordHash(Encoding.ASCII.GetBytes(PREV_PWD_SALT), PASSWORD5) });
            Passwords.Add(new DL_PreviousPassword
            { Salt = PREV_PWD_SALT,
                PasswordHash = security.GetPasswordHash(Encoding.ASCII.GetBytes(PREV_PWD_SALT), PASSWORD6) });

            return Passwords;
        }
    }
}
