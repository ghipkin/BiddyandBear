using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BB.

namespace BB.Mocks
{
    public class MockManageOrders : IManageOrders
    {
        ChangePasswordResponse IManageOrders.ChangePassword(ChangePasswordRequest request)
        {
            throw new NotImplementedException();
        }

        Task<ChangePasswordResponse> IManageOrders.ChangePasswordAsync(ChangePasswordRequest request)
        {
            throw new NotImplementedException();
        }

        PlaceOrderResponse IManageOrders.PlaceOrder(PlaceOrderRequest request)
        {
            throw new NotImplementedException();
        }

        Task<PlaceOrderResponse> IManageOrders.PlaceOrderAsync(PlaceOrderRequest request)
        {
            throw new NotImplementedException();
        }

        RegisterUserResponse IManageOrders.RegisterUser(RegisterCustomerRequest request)
        {
            throw new NotImplementedException();
        }

        Task<RegisterUserResponse> IManageOrders.RegisterUserAsync(RegisterCustomerRequest request)
        {
            throw new NotImplementedException();
        }

        RetrieveItemCategoriesResponse IManageOrders.RetrieveItemCategories(RetrieveItemCategoriesRequest request)
        {
            throw new NotImplementedException();
        }

        Task<RetrieveItemCategoriesResponse> IManageOrders.RetrieveItemCategoriesAsync(RetrieveItemCategoriesRequest request)
        {
            throw new NotImplementedException();
        }

        RetrieveItemsResponse IManageOrders.RetrieveItems(RetrieveItemsRequest request)
        {
            throw new NotImplementedException();
        }

        Task<RetrieveItemsResponse> IManageOrders.RetrieveItemsAsync(RetrieveItemsRequest request)
        {
            throw new NotImplementedException();
        }
    }
}