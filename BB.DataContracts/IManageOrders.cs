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

        [OperationBehavior]
        PlaceOrderResponse PlaceOrder(PlaceOrderRequest request);

        [OperationBehavior]
        RetrieveItemCategoriesResponse RetrieveItemCategories(RetrieveItemCategoriesRequest request);

        RetrieveItemsResponse RetrieveItems(RetrieveItemsRequest request);
    }
}
