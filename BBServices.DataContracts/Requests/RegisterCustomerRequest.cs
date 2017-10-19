using System.Runtime.Serialization;
using BB.DataLayer;

namespace BB.DataContracts
{
    [DataContract]
    public class RegisterCustomerRequest
    {
        public Customer NewCustomer { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }
    }

    [DataContract]
    public class ChangePasswordRequest
    {
        public Customer Customer { get; set; }

        public string NewPassword { get; set; }
    }

    [DataContract]
    public class LogonRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}