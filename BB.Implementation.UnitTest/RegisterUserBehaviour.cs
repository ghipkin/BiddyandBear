using System;
using System.Collections.Generic;
using BB.Implementation;
using BB.DataLayer;
using BB.DataContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BB.UnitTest
{
    [TestClass]
    public class RegisterUserBehaviour
    {
        #region constants
        private const string TITLE = "Mr.";
        private const string FIRST_NAME = "Nigel";
        private const string LAST_NAME = "Merkintheft";
        private const string ADDRESS_LINE_1= "33 Acacia avenue";
        private const string ADDRESS_LINE_2 = "brigadoon";
        private const string ADDRESS_LINE_3 = "Whimsyshire";
        private const string ADDRESS_LINE_4 = "Bonnie Scotland";
        private const string COUNTRY = "COUNTRY";
        private const string POSTCODE = "BR1 1AA";
        private const string HOME_PHONE= "0111 111111";
        private const string MOBILE_PHONE= "0791 111111";
        private const string EMAIL_ADDRESS= "nigel@merkintheft.com";

        private const int SOURCE_ID = 12;
        private const string SOURCE_NAME = "internet";
        private const string SOURCE_DESCRIPTION = "order from the internet";

        private const int ITEM_ID = 13;
        private const string ITEM_NAME = "widget";
        private const string ITEM_DESCRIPTION = "widget description";
        private const decimal ITEM_PRICE = 3.14m;

        #endregion

        //[TestMethod]
        //public void RegisterUserHappyPath()
        //{
        //    //ARRANGE
        //    var mock = new BBDataLayer_Mock();
        //    var svc = new BB.Implementation.BBService(mock);
        //    var customer = GetCustomer();
        //    var request = new RegisterCustomerRequest { NewCustomer = customer };

        //    //ACT
        //    var response = svc.RegisterUser(request);

        //    //ASSERT
        //    Assert.AreEqual(0, response.CallResult);
        //    Assert.IsTrue(String.IsNullOrEmpty(response.Message));
        //    Assert.IsTrue(mock.SaveChangesCalled);
        //}

        //[TestMethod]
        //public void RegisterUserFailure()
        //{
        //    //ARRANGE
        //    var mock = new BBDataLayer_Mock();
        //    mock.Failure = true;
        //    var svc = new Implementation.BBService(mock);
        //    var customer = GetCustomer();
        //    var request = new RegisterCustomerRequest { NewCustomer = customer };

        //    //ACT
        //    var response = svc.RegisterUser(request);

        //    //ASSERT
        //    Assert.AreEqual(BBDataLayer_Mock.FAILURE_RESULT, response.CallResult);
        //    Assert.IsFalse(String.IsNullOrEmpty(response.Message));
        //    Assert.IsTrue(mock.SaveChangesCalled);
        //    Assert.AreEqual(Implementation.BBService.REGISTER_CUSTOMER_SAVE_FAILED, response.Message);
            
        //}

        //[TestMethod]
        //public void RegisterUserException()
        //{
        //    //ARRANGE
        //    var mock = new BBDataLayer_Mock();
        //    mock.Exception = true;
        //    var svc = new Implementation.BBService(mock);
        //    var customer = GetCustomer();
        //    var request = new RegisterCustomerRequest { NewCustomer = customer };

        //    //ACT
        //    var response = svc.RegisterUser(request);

        //    //ASSERT
        //    Assert.AreEqual(1, response.CallResult);
        //    Assert.IsTrue(mock.SaveChangesCalled);
        //    Assert.IsFalse(String.IsNullOrEmpty(response.Message));
        //    Assert.IsTrue(response.Message.StartsWith(Implementation.BBService.REGISTER_CUSTOMER_SAVE_FAILED));
        //    Assert.IsTrue(response.Message.EndsWith(BBDataLayer_Mock.ERROR_MESSAGE));
        //}

        //[TestMethod]
        //public void PlaceOrderHappyPath()
        //{
        //    //ARRANGE
        //    var mock = new BBDataLayer_Mock();
        //    var svc = new Implementation.BBService(mock);
        //    var order = GetOrder();
        //    var request = new PlaceOrderRequest { NewOrder = order };

        //    //ACT
        //    var response = svc.PlaceOrder(request);

        //    //ASSERT
        //    Assert.AreEqual(0, response.CallResult);
        //    Assert.IsTrue(String.IsNullOrEmpty(response.Message));
        //    Assert.IsTrue(mock.SaveChangesCalled);
        //}

        //private Customer GetCustomer()
        //{
        //    var customer = new Customer();

        //    customer.Title = TITLE;
        //    customer.FirstName = FIRST_NAME;
        //    customer.LastName = LAST_NAME;
        //    customer.AddressLine1 = ADDRESS_LINE_1;
        //    customer.AddressLine2 = ADDRESS_LINE_2;
        //    customer.AddressLine3 = ADDRESS_LINE_3;
        //    customer.AddressLine4 = ADDRESS_LINE_4;
        //    customer.PostalCode = POSTCODE;
        //    customer.HomePhoneNo = HOME_PHONE;
        //    customer.MobilePhoneNo = MOBILE_PHONE;
        //    customer.EmailAddress = EMAIL_ADDRESS;

        //    return customer;
        //}

        //private Order GetOrder()
        //{
        //    var order = new Order();

        //    order.Cancelled = false;
        //    //order.Customer = GetCustomer();
        //    order.CustomerId = order.CustomerId
        //    order.DateOrderPlaced = DateTime.Now;
        //    //order.Source = GetSource();
        //    order.SourceId = order.SourceId;
        //    //order.OrderLines = GetOrderLines();

        //    return order;
        //}

        //private Source GetSource()
        //{
        //    var source = new Source();
        //    source.Description = SOURCE_DESCRIPTION;
        //    source.Name = SOURCE_NAME;
        //    source.Id = SOURCE_ID;

        //    return source;
        //}

        //private List<OrderLine> GetOrderLines()
        //{
        //    var orderlines = new List<OrderLine>();
        //    var orderline = new OrderLine();

        //    var item = new Item();
        //    item.Active = true;
        //    item.Description = ITEM_DESCRIPTION;
        //    item.Id = ITEM_ID;
        //    item.Name = ITEM_NAME;
        //    item.Price = ITEM_PRICE;

        //    orderline.Item = item;
        //    orderline.ItemId = item.Id;
        //    orderline.Number = 1;

        //    orderlines.Add(orderline);

        //    return orderlines;
        //}
    }
}
