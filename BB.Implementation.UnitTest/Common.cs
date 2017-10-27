using System;
using System.Collections.Generic;
using BB.DataContracts;

namespace BB.Implementation.UnitTest
{
    internal class Common
    {
        #region constants
        internal const long CUSTOMER_ID = 789464;
        internal const string TITLE = "Mr.";
        internal const string FIRST_NAME = "Nigel";
        internal const string LAST_NAME = "Merkintheft";
        internal const string ADDRESS_LINE_1 = "33 Acacia avenue";
        internal const string ADDRESS_LINE_2 = "brigadoon";
        internal const string ADDRESS_LINE_3 = "Whimsyshire";
        internal const string ADDRESS_LINE_4 = "Bonnie Scotland";
        internal const string COUNTRY = "COUNTRY";
        internal const string POSTCODE = "BR1 1AA";
        internal const string HOME_PHONE = "0111 111111";
        internal const string MOBILE_PHONE = "0791 111111";
        internal const string EMAIL_ADDRESS = "nigel@merkintheft.com";

        internal const string USERNAME = "nmerkintheft";
        internal const string PASSWORD = "password";
        internal const string PASSWORD_HASH = "password";
        internal static byte[] SALT = { (byte)1, (byte)2, (byte)3, (byte)4, (byte)5 };

        internal const int SOURCE_ID = 12;
        internal const string SOURCE_NAME = "internet";
        internal const string SOURCE_DESCRIPTION = "order from the internet";

        //Items
        internal const int ITEM1_ID = 13;
        internal const string ITEM1_NAME = "widget";
        internal const string ITEM1_DESCRIPTION = "widget description";
        internal const decimal ITEM1_PRICE = 3.14m;
        internal const int ITEM1_QUANTITY = 1;

        internal const int ITEM2_ID = 14;
        internal const string ITEM2_NAME = "splodgett";
        internal const string ITEM2_DESCRIPTION = "splodgett description";
        internal const decimal ITEM2_PRICE = 6.28m;
        internal const int ITEM2_QUANTITY = 3;

        internal const int ITEM3_ID = 15;
        internal const string ITEM3_NAME = "merkin";
        internal const string ITEM3_DESCRIPTION = "public wig";
        internal const decimal ITEM3_PRICE = 12.56m;
        internal const int ITEM3_QUANTITY = 9;

        //Categories
        internal const int CATEGORY1_ID = 15;
        internal const string CATEGORY1_NAME = "widgets";
        internal const string CATEGORY1_DESCRIPTION = "description of widget description";
        internal const bool CATEGORY1_VAT = true;

        internal const int CATEGORY2_ID = 16;
        internal const string CATEGORY2_NAME = "splodgetts";
        internal const string CATEGORY2_DESCRIPTION = "description of splodgett description";
        internal const bool CATEGORY2_VAT = false;
        #endregion

        internal static Customer GetCustomer()
        {
            var customer = new Customer();

            customer.Title = TITLE;
            customer.FirstName = FIRST_NAME;
            customer.LastName = LAST_NAME;
            customer.AddressLine1 = ADDRESS_LINE_1;
            customer.AddressLine2 = ADDRESS_LINE_2;
            customer.AddressLine3 = ADDRESS_LINE_3;
            customer.AddressLine4 = ADDRESS_LINE_4;
            customer.PostalCode = POSTCODE;
            customer.HomePhoneNo = HOME_PHONE;
            customer.MobilePhoneNo = MOBILE_PHONE;
            customer.EmailAddress = EMAIL_ADDRESS;

            return customer;
        }

        internal static Order GetOrder()
        {
            var order = new Order();

            order.Cancelled = false;
            order.CustomerId = CUSTOMER_ID;
            order.DateOrderPlaced = DateTime.Now;
            order.SourceId = SOURCE_ID;

            return order;
        }

        internal static List<OrderLine> GetOrderedItems()
        {
            var orderlines = new List<OrderLine>();
            var orderline1 = new OrderLine();
            orderline1.ItemId = ITEM1_ID;
            orderline1.Quantity = ITEM1_QUANTITY;


            var orderline2 = new OrderLine();
            orderline2.ItemId = ITEM2_ID;
            orderline2.Quantity = ITEM2_QUANTITY;
            orderlines.Add(orderline1);
            orderlines.Add(orderline2);

            return orderlines;
        }

        internal static Source GetSource()
        {
            var source = new Source();
            source.Description = SOURCE_DESCRIPTION;
            source.Name = SOURCE_NAME;
            //source.Id = SOURCE_ID;

            return source;
        }
    }
}
