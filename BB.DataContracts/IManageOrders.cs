using System.ServiceModel;
using BB.DataContracts;

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
