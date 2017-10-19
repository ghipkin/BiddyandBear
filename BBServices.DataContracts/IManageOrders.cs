using System.ServiceModel;
using BB.Contracts;

namespace BB.DataContracts
{
    [ServiceContract]
    public interface IManageOrders
    {
        [OperationBehavior]
        RegisterUserResponse RegisterUser(RegisterCustomerRequest request);

        [OperationBehavior]
        ChangePasswordResponse ChangePassword(ChangePasswordRequest request);
    }
}
