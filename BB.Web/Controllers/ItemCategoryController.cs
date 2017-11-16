using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BB.DataContracts;

namespace BB.Web.Controllers
{
    public class ItemCategoryController : Controller
    {
        // GET: ItemCategory
        public ActionResult Categories()
        {
            var BBServiceClient = Common.Instance.GetBBServiceClient();
            //var BBServiceClient = new ServiceReference1.ManageOrdersClient();
            var ItemCategories = BBServiceClient.RetrieveItemCategories(new RetrieveItemCategoriesRequest());
            
            return View(ItemCategories.Categories);
        }
    }
}