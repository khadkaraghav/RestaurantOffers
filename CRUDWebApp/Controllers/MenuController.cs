using CRUDWebApp.Models;
using CRUDWebApp.Models.DAL;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRUDWebApp.Controllers
{
    public class MenuController : Controller
    {
        IOrganizationService service = null;
        // GET: Offer
        public ActionResult Index()
        {
            DAL_Menu objDAL = new DAL_Menu();
            List<Menu> iteminfo = objDAL.RetriveRecords();
            ViewBag.iteminfo = iteminfo;
            return View();
        }


        public ActionResult AddNew(string id)
        {
            DAL_Menu objDAL = new DAL_Menu();
            // List<Microsoft.Xrm.Sdk.EntityReference> refUsers = objDAL.GetEntityReference();
            Menu objItemModel = new Menu();
            Guid itemId = Guid.Empty;
            if (id != null)
            {
                itemId = Guid.Parse(id);
            }

            if (itemId != Guid.Empty)
            {
                objItemModel = objDAL.getCurrentRecord(itemId);
            }

            //if (refUsers.Count > 0)

            //{

            //    ViewBag.EntityReferenceUsers = new SelectList(refUsers, "Id", "Name");

            //}

            return View(objItemModel);
        }


        public ActionResult Delete(Guid id)
        {
            service = CRMClass.GetCrmService();
            if (service != null)
            {
                service.Delete("rgv_menuitem", id);
            }
            return Redirect("~/Menu/Index");
        }

        [HttpPost]
        public ActionResult AddNew(Menu itemmodel)
        {
            DAL_Menu objDAL = new DAL_Menu();

            Guid id = itemmodel.ItemID;

            objDAL.SaveAccount(itemmodel);

            return Redirect("~/Menu/Index");
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}