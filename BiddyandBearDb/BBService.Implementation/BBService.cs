using System;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Data.Entity;
using BB.Implementation;
using BB.Contracts;
using BB.DataLayer;
using BB.Implementation.Config;

namespace BB.Implementation
{
    public class BBService : IManageOrders
    {
        public BBService()
        {

        }

        //Error messages
        public const string REGISTER_CUSTOMER_SAVE_FAILED = "Could not save Customer Details.";


        public RegisterUserResponse RegisterUser(RegisterCustomerRequest request)
        {
            var ct = request.NewCustomer;

            ct.Salt = Convert.ToBase64String(Security.GenerateNewSalt());

            DL_Customer CustormerToSave = 

            Context.Customers.Add(ct);
            int result = 0;
            try
            {
                result = Context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RegisterUserResponse { CallResult = 1, Message = REGISTER_CUSTOMER_SAVE_FAILED + "\n" + e.Message, MessageType = MessageType.Error };
            }

            if (result != 0)
            {
                return new RegisterUserResponse { CallResult = 1, Message = REGISTER_CUSTOMER_SAVE_FAILED, MessageType = MessageType.Error };
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
                throw new Exception(Security.PASSWORD_PREVIOUS_TO_CHECK_MISSING);
            }

            List<PreviousPassword> PreviousPasswords = null;

            if (NumberPrevPwdsToCheck > 0)
            {
                //get a list of the number previous passwords in the last six months
                //PreviousPasswords = request.Customer.PreviousPasswords.OrderByDescending(e=>e. Where(e => e.ExipirationDate > DateTime.Now.AddMonths(-6)).ToList<PreviousPassword>();
            }

            PasswordCheckResponse response = Security.CheckPassword(request.NewPassword, PreviousPasswords);

            if (!response.PasswordOK)
            {
                return new ChangePasswordResponse { CallResult = 1, Message = response.Message, MessageType = MessageType.Error };
            }

            return new ChangePasswordResponse { CallResult = 0 };
        }

        public PlaceOrderResponse PlaceOrder(PlaceOrderRequest request)
        {
            throw new NotImplementedException();
        }

        #region "private methods"


        #endregion
    }
}
