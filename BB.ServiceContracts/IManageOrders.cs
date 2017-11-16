using System.ServiceModel;
using BB.DataContracts;

namespace BB.ServiceContracts
{
    [ServiceContract]
    public interface IManageOrders
    {
        [OperationContract]
        RegisterUserResponse RegisterUser(RegisterCustomerRequest request);

        [OperationContract]
        ChangePasswordResponse ChangePassword(ChangePasswordRequest request);

        [OperationContract]
        PlaceOrderResponse PlaceOrder(PlaceOrderRequest request);

        [OperationContract]
        RetrieveItemCategoriesResponse RetrieveItemCategories(RetrieveItemCategoriesRequest request);

        [OperationContract]
        RetrieveItemsResponse RetrieveItems(RetrieveItemsRequest request);
    }
}
