using System;
using System.Collections.Generic;
using BB.DataLayer;
using BB.DataLayer.Abstract;
using BB.DataContracts;
using BB.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BB.Implementation.UnitTest
{
    [TestClass]
    public class RegisterUserBehaviour
    {

        [TestInitialize]
        public void Setup()
        {
            MockDatabase.MockedDb = new List<DatabaseRecord>();
        }

        [TestMethod]
        public void RegisterUserHappyPath()
        {
            //ARRANGE
            var MockCustomer = new MOCK_DL_Customer();
            
            var customer = Common.GetCustomer();
            var request = new RegisterCustomerRequest { NewCustomer = customer, UserName = Common.USERNAME, Password = Common.PASSWORD };

            var MockSecurity = new Mock<ISecurityMethods>();
            MockSecurity.Setup(x => x.GenerateNewSalt()).Returns(Common.SALT);
            MockSecurity.Setup(x => x.GetPasswordHash(It.IsAny<byte[]>(), It.IsAny<String>())).Returns(Common.PASSWORD_HASH);

            //create service object with mocks
            var svc = new BB.Implementation.BBService();
            svc.MockCustomer = MockCustomer;
            svc.MockSecurity = MockSecurity.Object;

            var s = svc.MockSecurity.GetPasswordHash(null, "");

            //ACT
            var response = svc.RegisterUser(request);

            //ASSERT
            Assert.AreEqual(0, response.CallResult);
            Assert.IsTrue(String.IsNullOrEmpty(response.Message));

            //check what was returned
            Assert.AreEqual(Common.SALT, MockCustomer.Salt);
            Assert.IsFalse(MockCustomer.PasswordNeedsChanging);
            Assert.AreEqual(Common.USERNAME, MockCustomer.UserName);
            Assert.AreEqual(Common.PASSWORD_HASH, MockCustomer.PasswordHash);
        }

        [TestMethod]
        public void RegisterUserFailure()
        {
            //ARRANGE
            var MockCustomer = new MOCK_DL_Customer();
            MockCustomer.fail = true;

            var customer = Common.GetCustomer();
            var request = new RegisterCustomerRequest { NewCustomer = customer, UserName = Common.USERNAME, Password = Common.PASSWORD };

            var MockSecurity = new Mock<ISecurityMethods>();
            MockSecurity.Setup(x => x.GenerateNewSalt()).Returns(Common.SALT);
            MockSecurity.Setup(x => x.GetPasswordHash(It.IsAny<byte[]>(), It.IsAny<String>())).Returns(Common.PASSWORD_HASH);

            //create service object with mocks
            var svc = new BB.Implementation.BBService();
            svc.MockCustomer = MockCustomer;
            svc.MockSecurity = MockSecurity.Object;

            var s = svc.MockSecurity.GetPasswordHash(null, "");

            //ACT
            var response = svc.RegisterUser(request);

            //ASSERT
            Assert.AreEqual(1, response.CallResult);
            Assert.IsFalse(String.IsNullOrEmpty(response.Message));
            Assert.IsTrue(response.Message.StartsWith(BBService.REGISTER_CUSTOMER_SAVE_FAILED));
            Assert.IsTrue(response.Message.EndsWith(MOCK_DL_Customer.ERR_SAVE_FAILED));
        }
    }
}
