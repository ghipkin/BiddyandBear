using System;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Transactions;
using BB.Implementation;
using BB.DataContracts;
using BB.DataLayer;
using BB.DataLayer.Abstract;
using BB.Implementation.Config;

namespace BB.Implementation
{
    public class BBService : IManageOrders
    {

        public BBService()
        {

        }

        internal MOCK_DL_Customer MockCustomer { get; set; }
        internal ISecurityMethods MockSecurity { get; set; }

        internal bool MockOrderLines { get; set; }
        internal bool MockCurrentItemCategories { get; set; }
        internal bool MockCurrentItems { get; set; }
        internal bool OrderLinesFail { get; set; }
        internal bool CurrentItemCategoriesFail { get; set; }
        internal bool CurrentItemsFail { get; set; }

        internal ADL_Mapping MappingObject { get; set; }

        //Error messages
        public const string REGISTER_CUSTOMER_SAVE_FAILED = "Could not save Customer Details.";
        public const string PLACE_ORDER_NO_ITEMS = "No Items in order.";
        public const string PLACE_ORDER_ZERO_CUSTOMERID = "CustomerID cannot be zero.";
        public const string RETRIEVE_ITEMS_NO_CATEGORYID = "No CategoryID passed.";


        public RegisterUserResponse RegisterUser(RegisterCustomerRequest request)
        {
            var ct = request.NewCustomer;

            //convert Customer to DLCustomer
            ADL_Customer CustomerToSave;
            if (MockCustomer == null)
            {
                CustomerToSave = GetMappingObject().MapCustomertoDLCustomer(ct);
            }
            else
            {
                CustomerToSave = MockCustomer;
            }

            ISecurityMethods security;
            if(MockSecurity == null)
            {
                security = new SecurityMethods();
            }
            else
            {
                security = MockSecurity;
            }

            //Add additional fields 
            byte[] Salt = security.GenerateNewSalt();
            CustomerToSave.Salt = Salt;
            CustomerToSave.PasswordNeedsChanging = false;
            CustomerToSave.UserName = request.UserName;
            CustomerToSave.PasswordHash = security.GetPasswordHash(Salt, request.Password);

            try
            {
                CustomerToSave.Save();
            }
            catch (Exception e)
            {
                return new RegisterUserResponse { CallResult = 1, Message = REGISTER_CUSTOMER_SAVE_FAILED + "\n" + e.Message, MessageType = MessageType.Error };
            }

            return new RegisterUserResponse { CallResult = 0 };
        }

        public ChangePasswordResponse ChangePassword(ChangePasswordRequest request)
        {
            var cust = request.Customer;

            var SecuritySettings = (SecuritySection)ConfigurationManager.GetSection("passwordPolicies");

            var PrevPwdsPolicy = SecuritySettings.PasswordPolicies["PreviousPwdsToCheck"];

            int NumberPrevPwdsToCheck = 0;
            if (!int.TryParse(PrevPwdsPolicy.value, out NumberPrevPwdsToCheck))
            {
                throw new Exception(SecurityMethods.PASSWORD_PREVIOUS_TO_CHECK_MISSING);
            }

            var security = new SecurityMethods();
            PasswordCheckResponse PwCheckResponse;
            if (NumberPrevPwdsToCheck > 0)
            {
                //get a list of the number previous passwords in the last six months
                //get the previous passwords
                var PasswordQuery = new Dictionary<String, Object>();
                PasswordQuery.Add("CustomerId", request.Customer.Id);
                var PreviousPasswords = new DL_PreviousPasswords();
                PreviousPasswords.LoadRecords(PasswordQuery);
                //PreviousPasswords = request.Customer.PreviousPasswords.OrderByDescending(e=>e. Where(e => e.ExipirationDate > DateTime.Now.AddMonths(-6)).ToList<PreviousPassword>();
                PwCheckResponse = security.CheckPassword(request.NewPassword,
                    PreviousPasswords.PreviousPasswords.OrderByDescending(x=>x.CreationDate).Take(NumberPrevPwdsToCheck));
            }

            PwCheckResponse = security.CheckPassword(request.NewPassword, null);

            if (!PwCheckResponse.PasswordOK)
            {
                return new ChangePasswordResponse { CallResult = 1, Message = PwCheckResponse.Message, MessageType = MessageType.Error };
            }

            return new ChangePasswordResponse { CallResult = 0 };
        }

        public PlaceOrderResponse PlaceOrder(PlaceOrderRequest request)
        {
            if(request.NewOrder.CustomerId == 0)
            {
                return new PlaceOrderResponse { CallResult = 1, Message = PLACE_ORDER_ZERO_CUSTOMERID, MessageType = MessageType.Error };
            }

            if (request.OrderedItems == null || request.OrderedItems.Count == 0)
            {
                return new PlaceOrderResponse { CallResult = 1, Message = PLACE_ORDER_NO_ITEMS, MessageType = MessageType.Error };
            }

            //Create Order and OrderLines objects to save
            var OrderRecord = GetMappingObject().MapOrdertoDLOrder(request.NewOrder);

            var Orderlines = Get_DL_OrderLines();
            foreach (var item in request.OrderedItems)
            {
                var Orderline = Get_DL_OrderLine(item.ItemId, item.Quantity, OrderRecord.Id);
                Orderlines.OrderLines.Add(Orderline);
            }

            var Connection = new Connection();

            using (var scope = new TransactionScope())
            {
                using (SqlConnection cn = Connection.GetConnection())
                {
                    try
                    {
                        OrderRecord.Save(cn);
                        //Add the newly created order ID to each item
                        foreach(ADL_OrderLine orderline in Orderlines.OrderLines)
                        {
                            orderline.OrderId = OrderRecord.Id;
                        }
                        Orderlines.SaveRecords(cn);
                    }
                    catch (Exception e)
                    {
                        return new PlaceOrderResponse { CallResult = 1, Message = e.Message, MessageType = MessageType.Error };
                    }
                }
                scope.Complete();
            }
            
            return new PlaceOrderResponse { CallResult = 0 };
        }

        public RetrieveItemCategoriesResponse RetrieveItemCategories(RetrieveItemCategoriesRequest request)
        {

            var Categories = Get_DL_CurrentItemCategories();
            
            try
            {
                Categories.LoadRecords(null);
            }
            catch(Exception e)
            {
                return new RetrieveItemCategoriesResponse { CallResult = 1, Message = e.Message, MessageType = MessageType.Error };
            }

            var result = new List<BB.DataContracts.CurrentItemCategory>();
            foreach(var DLCat in Categories.CurrentItemCategories)
            {
                result.Add(GetMappingObject().MapCurrentItemCategoryfromDLCurrentItemCategory(DLCat));
            }

            return new RetrieveItemCategoriesResponse { Categories = result, CallResult = 0 };
        }

        public RetrieveItemsResponse RetrieveItems(RetrieveItemsRequest request)
        {

            if(request.CategoryId == 0)
            {
                return new RetrieveItemsResponse { CallResult = 1, Message = RETRIEVE_ITEMS_NO_CATEGORYID, MessageType = MessageType.Error };
            }

            var Items = Get_DL_CurrentItems();

            var query = new Dictionary<String, object>();
            query.Add("Id", request.CategoryId);

            try
            {
                Items.LoadRecords(query);
            }
            catch (Exception e)
            {
                return new RetrieveItemsResponse { CallResult = 1, Message = e.Message, MessageType = MessageType.Error };
            }

            var result = new List<BB.DataContracts.CurrentItem>();
            foreach (var DLItem in Items.CurrentItems)
            {
                result.Add(GetMappingObject().MapCurrentItemfromDLCurrentItem(DLItem));
            }

            return new RetrieveItemsResponse { Items = result, CallResult = 0 };
        }

        #region "private methods"
        private ADL_Mapping GetMappingObject()
        {
            if (MappingObject == null)
            {
                MappingObject = new DLMapping();
            }

            return MappingObject;
        }

        private ADL_OrderLine Get_DL_OrderLine(long ItemId, int quantity, long OrderId)
        {
            ADL_OrderLine result;

            if(MockOrderLines)
            {
                result = new MOCK_DL_OrderLine();
            }
            else
            {
                result = new DL_OrderLine();
            }

            result.ItemId = ItemId;
            result.Quantity = quantity;
            result.OrderId = OrderId;

            return result;
        }

        private ADL_OrderLines Get_DL_OrderLines()
        {
            ADL_OrderLines result;

            if (MockOrderLines)
            {
                result = new MOCK_DL_OrderLines();
                if(OrderLinesFail)
                {
                    ((MOCK_DL_OrderLines)result).Failure = true;
                }
            }
            else
            {
                result = new DL_OrderLines();
            }

            return result;
        }

        private ADL_CurrentItemCategories Get_DL_CurrentItemCategories()
        {
            ADL_CurrentItemCategories result;

            if (MockCurrentItemCategories)
            {
                result = new MOCK_DL_CurrentItemCategories();
                if (CurrentItemCategoriesFail)
                {
                    ((MOCK_DL_CurrentItemCategories)result).Failure = true;
                }
            }
            else
            {
                result = new DL_CurrentItemCategories();
            }

            return result;
        }

        private ADL_CurrentItems Get_DL_CurrentItems()
        {
            ADL_CurrentItems result;

            if (MockCurrentItems)
            {
                result = new MOCK_DL_CurrentItems();
                if (CurrentItemsFail)
                {
                    ((MOCK_DL_CurrentItems)result).Failure = true;
                }
            }
            else
            {
                result = new DL_CurrentItems();
            }

            return result;
        }

        #endregion
    }
}
