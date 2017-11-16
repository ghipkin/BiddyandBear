using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BB.ServiceContracts;
using BB.DataContracts;

namespace BB.ServiceMocks
{
    public class MockManageOrders : IManageOrders
    {
        ChangePasswordResponse IManageOrders.ChangePassword(ChangePasswordRequest request)
        {
            throw new NotImplementedException();
        }

        PlaceOrderResponse IManageOrders.PlaceOrder(PlaceOrderRequest request)
        {
            throw new NotImplementedException();
        }

        RegisterUserResponse IManageOrders.RegisterUser(RegisterCustomerRequest request)
        {
            throw new NotImplementedException();
        }

        RetrieveItemCategoriesResponse IManageOrders.RetrieveItemCategories(RetrieveItemCategoriesRequest request)
        {
            throw new NotImplementedException();
        }

        RetrieveItemsResponse IManageOrders.RetrieveItems(RetrieveItemsRequest request)
        {
            throw new NotImplementedException();
        }
    }
}