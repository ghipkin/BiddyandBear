using System;
using System.Collections.Generic;
using System.Linq;
using BB.Implementation;
using BB.DataLayer;
using BB.DataLayer.Abstract;
using BB.DataContracts;
using BB.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BB.Implementation.UnitTest
{
    [TestClass]
    public class PlaceOrderBehaviour
    {
        [TestInitialize]
        public void Setup()
        {
            MockDatabase.MockedDb = new List<DatabaseRecord>();
        }

        [TestMethod]
        public void PlaceOrderHappyPath()
        {
            //ARRANGE
            var svc = new Implementation.BBService();
            svc.MockOrderLines = true;
            svc.MappingObject = new Mock_DLMapping();
            var order = Common.GetOrder();
            var orderlines = Common.GetOrderedItems();
            var request = new PlaceOrderRequest { NewOrder = order, OrderedItems = orderlines };

            //ACT
            var response = svc.PlaceOrder(request);

            //ASSERT
            Assert.AreEqual(0, response.CallResult);
            Assert.IsTrue(String.IsNullOrEmpty(response.Message));

            //Check what was sent to the database
            Assert.AreEqual(3, MockDatabase.MockedDb.Count);
            var SavedOrder = (ADL_Order)MockDatabase.MockedDb.Where(x => x.GetType() == typeof(MOCK_DL_Order)).SingleOrDefault();
            Assert.AreEqual(Common.CUSTOMER_ID, SavedOrder.CustomerId);
            Assert.AreEqual(Common.SOURCE_ID, SavedOrder.SourceId);
            Assert.IsFalse(SavedOrder.Cancelled);
            Assert.AreEqual(MOCK_DL_Order.DEFAULT_ORDER_ID, SavedOrder.Id);

            var SavedOrderLine1 = (MOCK_DL_OrderLine)MockDatabase.MockedDb.Where(x => x.GetType() == typeof(MOCK_DL_OrderLine) && ((MOCK_DL_OrderLine)x).ItemId == Common.ITEM1_ID).SingleOrDefault();
            Assert.AreEqual(Common.ITEM1_QUANTITY, SavedOrderLine1.Quantity);
            Assert.AreEqual(MOCK_DL_Order.DEFAULT_ORDER_ID, SavedOrderLine1.OrderId);
            var SavedOrderLine2 = (MOCK_DL_OrderLine)MockDatabase.MockedDb.Where(x => x.GetType() == typeof(MOCK_DL_OrderLine) && ((MOCK_DL_OrderLine)x).ItemId == Common.ITEM2_ID).SingleOrDefault();
            Assert.AreEqual(Common.ITEM2_QUANTITY, SavedOrderLine2.Quantity);
            Assert.AreEqual(MOCK_DL_Order.DEFAULT_ORDER_ID, SavedOrderLine2.OrderId);
        }

        [TestMethod]
        public void PlaceOrderHappyPath_SaveFails()
        {
            //ARRANGE
            var svc = new Implementation.BBService();
            svc.MockOrderLines = true;
            svc.OrderLinesFail = true;
            svc.MappingObject = new Mock_DLMapping();
            var order = Common.GetOrder();
            var orderlines = Common.GetOrderedItems();
            var request = new PlaceOrderRequest { NewOrder = order, OrderedItems = orderlines };

            //ACT
            var response = svc.PlaceOrder(request);

            //ASSERT
            Assert.AreNotEqual(0, response.CallResult);
            Assert.IsFalse(String.IsNullOrEmpty(response.Message));

            Assert.AreEqual(DataContracts.MessageType.Error, response.MessageType);
            Assert.AreEqual(MOCK_DL_OrderLines.ERR_FAILURE, response.Message);
        }

        [TestMethod]
        public void PlaceOrderHappyPath_NoOrderlines()
        {
            //ARRANGE
            var svc = new Implementation.BBService();
            var order = Common.GetOrder();
            var orderlines = Common.GetOrderedItems();
            var request = new PlaceOrderRequest { NewOrder = order, OrderedItems = null };

            //ACT
            var response = svc.PlaceOrder(request);

            //ASSERT
            Assert.AreNotEqual(0, response.CallResult);
            Assert.IsFalse(String.IsNullOrEmpty(response.Message));

            Assert.AreEqual(BBService.PLACE_ORDER_NO_ITEMS, response.Message);
        }

        [TestMethod]
        public void PlaceOrderHappyPath_NoCustomer()
        {
            //ARRANGE
            var svc = new Implementation.BBService();
            var order = Common.GetOrder();
            order.CustomerId = 0;
            var orderlines = Common.GetOrderedItems();
            var request = new PlaceOrderRequest { NewOrder = order, OrderedItems = orderlines };

            //ACT
            var response = svc.PlaceOrder(request);

            //ASSERT
            Assert.AreNotEqual(0, response.CallResult);
            Assert.IsFalse(String.IsNullOrEmpty(response.Message));

            Assert.AreEqual(BBService.PLACE_ORDER_ZERO_CUSTOMERID, response.Message);
        }
    }
}
