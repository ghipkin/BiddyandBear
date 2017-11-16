﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BB.Web.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IManageOrders")]
    public interface IManageOrders {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IManageOrders/RegisterUser", ReplyAction="http://tempuri.org/IManageOrders/RegisterUserResponse")]
        BB.DataContracts.RegisterUserResponse RegisterUser(BB.DataContracts.RegisterCustomerRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IManageOrders/RegisterUser", ReplyAction="http://tempuri.org/IManageOrders/RegisterUserResponse")]
        System.Threading.Tasks.Task<BB.DataContracts.RegisterUserResponse> RegisterUserAsync(BB.DataContracts.RegisterCustomerRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IManageOrders/ChangePassword", ReplyAction="http://tempuri.org/IManageOrders/ChangePasswordResponse")]
        BB.DataContracts.ChangePasswordResponse ChangePassword(BB.DataContracts.ChangePasswordRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IManageOrders/ChangePassword", ReplyAction="http://tempuri.org/IManageOrders/ChangePasswordResponse")]
        System.Threading.Tasks.Task<BB.DataContracts.ChangePasswordResponse> ChangePasswordAsync(BB.DataContracts.ChangePasswordRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IManageOrders/PlaceOrder", ReplyAction="http://tempuri.org/IManageOrders/PlaceOrderResponse")]
        BB.DataContracts.PlaceOrderResponse PlaceOrder(BB.DataContracts.PlaceOrderRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IManageOrders/PlaceOrder", ReplyAction="http://tempuri.org/IManageOrders/PlaceOrderResponse")]
        System.Threading.Tasks.Task<BB.DataContracts.PlaceOrderResponse> PlaceOrderAsync(BB.DataContracts.PlaceOrderRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IManageOrders/RetrieveItemCategories", ReplyAction="http://tempuri.org/IManageOrders/RetrieveItemCategoriesResponse")]
        BB.DataContracts.RetrieveItemCategoriesResponse RetrieveItemCategories(BB.DataContracts.RetrieveItemCategoriesRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IManageOrders/RetrieveItemCategories", ReplyAction="http://tempuri.org/IManageOrders/RetrieveItemCategoriesResponse")]
        System.Threading.Tasks.Task<BB.DataContracts.RetrieveItemCategoriesResponse> RetrieveItemCategoriesAsync(BB.DataContracts.RetrieveItemCategoriesRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IManageOrders/RetrieveItems", ReplyAction="http://tempuri.org/IManageOrders/RetrieveItemsResponse")]
        BB.DataContracts.RetrieveItemsResponse RetrieveItems(BB.DataContracts.RetrieveItemsRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IManageOrders/RetrieveItems", ReplyAction="http://tempuri.org/IManageOrders/RetrieveItemsResponse")]
        System.Threading.Tasks.Task<BB.DataContracts.RetrieveItemsResponse> RetrieveItemsAsync(BB.DataContracts.RetrieveItemsRequest request);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IManageOrdersChannel : BB.Web.ServiceReference1.IManageOrders, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ManageOrdersClient : System.ServiceModel.ClientBase<BB.Web.ServiceReference1.IManageOrders>, BB.Web.ServiceReference1.IManageOrders {
        
        public ManageOrdersClient() {
        }
        
        public ManageOrdersClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ManageOrdersClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ManageOrdersClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ManageOrdersClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public BB.DataContracts.RegisterUserResponse RegisterUser(BB.DataContracts.RegisterCustomerRequest request) {
            return base.Channel.RegisterUser(request);
        }
        
        public System.Threading.Tasks.Task<BB.DataContracts.RegisterUserResponse> RegisterUserAsync(BB.DataContracts.RegisterCustomerRequest request) {
            return base.Channel.RegisterUserAsync(request);
        }
        
        public BB.DataContracts.ChangePasswordResponse ChangePassword(BB.DataContracts.ChangePasswordRequest request) {
            return base.Channel.ChangePassword(request);
        }
        
        public System.Threading.Tasks.Task<BB.DataContracts.ChangePasswordResponse> ChangePasswordAsync(BB.DataContracts.ChangePasswordRequest request) {
            return base.Channel.ChangePasswordAsync(request);
        }
        
        public BB.DataContracts.PlaceOrderResponse PlaceOrder(BB.DataContracts.PlaceOrderRequest request) {
            return base.Channel.PlaceOrder(request);
        }
        
        public System.Threading.Tasks.Task<BB.DataContracts.PlaceOrderResponse> PlaceOrderAsync(BB.DataContracts.PlaceOrderRequest request) {
            return base.Channel.PlaceOrderAsync(request);
        }
        
        public BB.DataContracts.RetrieveItemCategoriesResponse RetrieveItemCategories(BB.DataContracts.RetrieveItemCategoriesRequest request) {
            return base.Channel.RetrieveItemCategories(request);
        }
        
        public System.Threading.Tasks.Task<BB.DataContracts.RetrieveItemCategoriesResponse> RetrieveItemCategoriesAsync(BB.DataContracts.RetrieveItemCategoriesRequest request) {
            return base.Channel.RetrieveItemCategoriesAsync(request);
        }
        
        public BB.DataContracts.RetrieveItemsResponse RetrieveItems(BB.DataContracts.RetrieveItemsRequest request) {
            return base.Channel.RetrieveItems(request);
        }
        
        public System.Threading.Tasks.Task<BB.DataContracts.RetrieveItemsResponse> RetrieveItemsAsync(BB.DataContracts.RetrieveItemsRequest request) {
            return base.Channel.RetrieveItemsAsync(request);
        }
    }
}