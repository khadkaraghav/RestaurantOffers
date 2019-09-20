using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRUDWebApp.Models;
using Microsoft.Xrm.Sdk;
using CRUDWebApp.Models.DAL;

namespace CRUDWebApp.Controllers
{
    public class OfferController : Controller
    {

        IOrganizationService service = null;
        // GET: Offer
        public ActionResult Index()
        {
            DAL_Offer objDAL = new DAL_Offer();
            List<Offer> offerinfo = objDAL.RetriveRecords();
            ViewBag.offerinfo = offerinfo;
            return View();
        }


        public ActionResult AddNew(string id)
        {
            DAL_Offer objDAL = new DAL_Offer();
            List<Microsoft.Xrm.Sdk.EntityReference> refMenuItems = objDAL.GetEntityReference();
            Offer objOfferModel = new Offer();
            Guid offerId = Guid.Empty;
            if (id != null)
            {
                offerId = Guid.Parse(id);
            }

            if (offerId != Guid.Empty)
            {
                objOfferModel = objDAL.getCurrentRecord(offerId);
            }

            if (refMenuItems.Count > 0)
            {
                ViewBag.EntityReferenceUsers = new SelectList(refMenuItems, "Id", "Name");
            }

            return View(objOfferModel);
        }


        public ActionResult Delete(Guid id)
        {
            service = CRMClass.GetCrmService();
            if (service != null)
            {
                service.Delete("rgv_offer", id);
            }
            return Redirect("~/Offer/Index");
        }

        [HttpPost]
        public ActionResult AddNew(Offer offermodel)
        {
            DAL_Offer objDAL = new DAL_Offer();

            Guid id = offermodel.OfferID;

            objDAL.SaveAccount(offermodel);

            return Redirect("~/Offer/Index");
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