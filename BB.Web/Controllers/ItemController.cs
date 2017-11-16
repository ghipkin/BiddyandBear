using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BB.DataContracts;

namespace BB.Web.Controllers
{
    public class ItemController : Controller
    {
        // GET: Item
        public ActionResult Items(long CategoryId)
        {
            var BBServiceClient = Common.Instance.GetBBServiceClient();
            
            var request = new DataContracts.RetrieveItemsRequest();
            request.CategoryId = CategoryId;
            var Items = BBServiceClient.RetrieveItems(request);

            return View(Items.Items);
        }
    }
}