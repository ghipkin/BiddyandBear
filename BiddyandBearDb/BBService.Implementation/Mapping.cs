using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BB.DataContracts;
using BB.DataLayer;

namespace BB.Implementation
{
    static class Mapping
    {
        public DL_Customer MapCustomerToDLCustomer(Customer customer)
        {
            var dlcustomer = new DL_Customer();

            dlcustomer.Title = customer.Title;
            dlcustomer.FirstName = customer.FirstName;
            dlcustomer.LastName = customer.LastName;
            dlcustomer.HomePhoneNo = customer.HomePhoneNo;
            dlcustomer.MobilePhoneNo = customer.MobilePhoneNo;
            dlcustomer.PasswordHash = customer.PasswordHash;
            dlcustomer.Salt = customer.Salt;
            dlcustomer.AddressLine1 = customer.AddressLine1;
            dlcustomer.AddressLine2 = customer.AddressLine2;
            dlcustomer.AddressLine3 = customer.AddressLine3;
            dlcustomer.AddressLine4 = customer.AddressLine4;
            dlcustomer.PostalCode = customer.PostalCode;
            dlcustomer.UserId = customer.UserId;
            dlcustomer.PasswordNeedsChanging = customer.PasswordNeedsChanging;

            return dlcustomer;
        }
    }
}
