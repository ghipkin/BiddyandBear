using System;
using System.Linq;
using System.Collections.Generic;
using BB.DataLayer;
using BB.DataLayer.Abstract;
using BB.Mocks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BB.Implementation.UnitTest
{
    [TestClass]
    public class RetrieveDataItemBehaviour
    {
        [TestInitialize]
        public void Setup()
        {
            MockDatabase.MockedDb = new List<DatabaseRecord>();
        }

        [TestMethod]
        public void RetrieveItemCategory_HappyPath()
        {
            //ARRANGE
            var svc = new Implementation.BBService();
            svc.MockCurrentItemCategories = true;

            var category1 = new DL_CurrentItemCategory();
            category1.Id = Common.CATEGORY1_ID;
            category1.Description = Common.CATEGORY1_DESCRIPTION;
            category1.Name = Common.CATEGORY1_NAME;
            category1.VAT = Common.CATEGORY1_VAT;

            var category2 = new DL_CurrentItemCategory();
            category2.Id = Common.CATEGORY2_ID;
            category2.Description = Common.CATEGORY2_DESCRIPTION;
            category2.Name = Common.CATEGORY2_NAME;
            category2.VAT = Common.CATEGORY2_VAT;

            MockDatabase.MockedDb.Add(category1);
            MockDatabase.MockedDb.Add(category2);

            //ACT
            var result = svc.RetrieveItemCategories(new DataContracts.RetrieveItemCategoriesRequest());

            //ASSERT
            Assert.AreEqual(0, result.CallResult);
            Assert.IsTrue(String.IsNullOrEmpty(result.Message));

            var returnedCategory1 = (DataContracts.CurrentItemCategory)result.Categories.Where(x => x.Id == Common.CATEGORY1_ID).SingleOrDefault();
            Assert.AreEqual(Common.CATEGORY1_NAME, returnedCategory1.Name);
            Assert.AreEqual(Common.CATEGORY1_DESCRIPTION, returnedCategory1.Description);
            Assert.AreEqual(Common.CATEGORY1_VAT, returnedCategory1.VAT);

            var returnedCategory2 = (DataContracts.CurrentItemCategory)result.Categories.Where(x => x.Id == Common.CATEGORY2_ID).SingleOrDefault();
            Assert.AreEqual(Common.CATEGORY2_NAME, returnedCategory2.Name);
            Assert.AreEqual(Common.CATEGORY2_DESCRIPTION, returnedCategory2.Description);
            Assert.AreEqual(Common.CATEGORY2_VAT, returnedCategory2.VAT);
        }

        [TestMethod]
        public void RetrieveItemCategories_Failure()
        {
            //ARRANGE
            var svc = new Implementation.BBService();
            svc.MockCurrentItemCategories = true;
            svc.CurrentItemCategoriesFail = true;

            //ACT
            var result = svc.RetrieveItemCategories(new DataContracts.RetrieveItemCategoriesRequest());

            //ASSERT
            Assert.AreNotEqual(0, result.CallResult);
            Assert.IsFalse(String.IsNullOrEmpty(result.Message));

            Assert.AreEqual(DataContracts.MessageType.Error, result.MessageType);
            Assert.AreEqual(MOCK_DL_CurrentItemCategories.ERR_FAILURE, result.Message);
        }

        [TestMethod]
        public void RetrieveItems_HappyPath()
        {
            //ARRANGE
            var svc = new Implementation.BBService();
            svc.MockCurrentItems = true;

            var Item1 = new DL_CurrentItem();
            Item1.Id = Common.ITEM1_ID;
            Item1.Name = Common.ITEM1_NAME;
            Item1.Description = Common.ITEM1_DESCRIPTION;
            Item1.CategoryId = Common.CATEGORY1_ID;
            Item1.Price = Common.ITEM1_PRICE;
            

            var Item2 = new DL_CurrentItem();
            Item2.Id = Common.ITEM2_ID;
            Item2.Name = Common.ITEM2_NAME;
            Item2.Description = Common.ITEM2_DESCRIPTION;
            Item2.CategoryId = Common.CATEGORY1_ID;
            Item2.Price = Common.ITEM2_PRICE;

            MockDatabase.MockedDb.Add(Item1);
            MockDatabase.MockedDb.Add(Item2);

            //ACT
            var request = new DataContracts.RetrieveItemsRequest { CategoryId = Common.CATEGORY1_ID };
            var result = svc.RetrieveItems(request);

            //ASSERT
            Assert.AreEqual(0, result.CallResult);
            Assert.IsTrue(String.IsNullOrEmpty(result.Message));
            Assert.AreEqual(2, result.Items.Count);//Should be only two results;

            var returnedItem1 = (DataContracts.CurrentItem)result.Items.Where(x => x.Id == Common.ITEM1_ID).SingleOrDefault();
            Assert.AreEqual(Common.CATEGORY1_ID, returnedItem1.CategoryId);
            Assert.AreEqual(Common.ITEM1_NAME, returnedItem1.Name);
            Assert.AreEqual(Common.ITEM1_DESCRIPTION, returnedItem1.Description);
            Assert.AreEqual(Common.ITEM1_PRICE, returnedItem1.Price);

            var returnedItem2 = (DataContracts.CurrentItem)result.Items.Where(x => x.Id == Common.ITEM2_ID).SingleOrDefault();
            Assert.AreEqual(Common.CATEGORY1_ID, returnedItem2.CategoryId);
            Assert.AreEqual(Common.ITEM2_NAME, returnedItem2.Name);
            Assert.AreEqual(Common.ITEM2_DESCRIPTION, returnedItem2.Description);
            Assert.AreEqual(Common.ITEM2_PRICE, returnedItem2.Price);
        }

        [TestMethod]
        public void RetrieveItems_Failure()
        {
            //ARRANGE
            var svc = new Implementation.BBService();
            svc.MockCurrentItems = true;
            svc.CurrentItemsFail = true;

            //ACT
            var request = new DataContracts.RetrieveItemsRequest { CategoryId = Common.CATEGORY1_ID };
            var result = svc.RetrieveItems(request);

            //ASSERT
            Assert.AreNotEqual(0, result.CallResult);
            Assert.IsFalse(String.IsNullOrEmpty(result.Message));

            Assert.AreEqual(DataContracts.MessageType.Error, result.MessageType);
            Assert.AreEqual(MOCK_DL_CurrentItems.ERR_FAILURE, result.Message);
        }
    }
}
