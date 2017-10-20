using System;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Data.Entity;
using BB.Implementation;
using BB.DataContracts;
using BB.DataLayer;
using BB.Implementation.Config;

namespace BB.Implementation
{
    public class BBService : IManageOrders
    {

        public BBService()
        {

        }

        internal IDatabaseRecord MockCustomer { get; set; }
        internal ISecurityMethods MockSecurity { get; set; }

        //Error messages
        public const string REGISTER_CUSTOMER_SAVE_FAILED = "Could not save Customer Details.";


        public RegisterUserResponse RegisterUser(RegisterCustomerRequest request)
        {
            var ct = request.NewCustomer;

            //convert Customer to DLCustomer
            DL_Customer CustomerToSave;
            if (MockCustomer == null)
            {
                CustomerToSave = DLMapping.MapCustomertoDLCustomer(ct);
            }
            else
            {
                CustomerToSave = (DL_Customer)MockCustomer;
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
            var Security = new SecurityMethods();
            byte[] Salt = Security.GenerateNewSalt();
            CustomerToSave.Salt = Salt;
            CustomerToSave.PasswordNeedsChanging = false;
            CustomerToSave.UserName = request.UserName;
            CustomerToSave.PasswordHash = Security.GetPasswordHash(Salt, request.Password);

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
                PwCheckResponse = security.CheckPassword(request.NewPassword, PreviousPasswords.Records.OrderByDescending(x=>x.CreationDate).Take(NumberPrevPwdsToCheck).ToList<DL_PreviousPassword>());
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
            throw new NotImplementedException();
        }

        #region "private methods"


        #endregion
    }
}
