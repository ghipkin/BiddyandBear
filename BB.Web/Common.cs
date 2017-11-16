using System;
using System.ServiceModel;
using System.Configuration;
using BB.ServiceContracts;
using BB.ServiceMocks;

public class Common
{
    internal bool MockBBService { get; set; }

    private static Common instance;

    private Common() { }

    public static Common Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Common();
            }
            return instance;
        }
    }

    public IManageOrders GetBBServiceClient()
    {
        if(MockBBService)
        {
            return new MockManageOrders();
        }
        else
        {
            WSHttpBinding binding = new WSHttpBinding();
            binding.Name = "BBService";
            var Address = new EndpointAddress(ConfigurationManager.AppSettings["BBServiceEndpoint"]);//  "http://it500574.ho.pfgroup.provfin.com:209/BBService.svc/BBService.svc");
            ChannelFactory <IManageOrders> factory = new ChannelFactory<IManageOrders>(binding, Address);
            

            return factory.CreateChannel();
        }
    }
}