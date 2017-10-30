using System;
using System.Runtime.Serialization;

namespace BB.DataContracts
{
    [DataContract]
    public class RegisterUserResponse : ServiceResponse
    {
    }

    [DataContract]
    public class ChangePasswordResponse : ServiceResponse
    { }
}