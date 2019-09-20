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
    public class HomeController : Controller
    {
        IOrganizationService service = null;
        public ActionResult Index()
        {
            DAL_AccountEntity objDAL = new DAL_AccountEntity();
            List<AccountEntityModels> accountinfo = objDAL.RetriveRecords();
            ViewBag.accountinfo = accountinfo;
            return View();
        }
        public ActionResult AddNew(string id)
        {
            DAL_AccountEntity objDAL = new DAL_AccountEntity();
            // List<Microsoft.Xrm.Sdk.EntityReference> refUsers = objDAL.GetEntityReference();
            AccountEntityModels objAccountModel = new AccountEntityModels();
            Guid accountId = Guid.Empty;
            if (id != null)
            {
                accountId = Guid.Parse(id);
            }

            if (accountId != Guid.Empty)
            {
                objAccountModel = objDAL.getCurrentRecord(accountId);
            }

            //if (refUsers.Count > 0)

            //{

            //    ViewBag.EntityReferenceUsers = new SelectList(refUsers, "Id", "Name");

            //}

            return View(objAccountModel);
        }
        public ActionResult Delete(Guid id)
        {
            service = CRMClass.GetCrmService();
            if (service != null)
            {
                service.Delete("rgv_customer", id);
            }
            return Redirect("~/Home");
        }
        [HttpPost]
        public ActionResult AddNew(AccountEntityModels accountdmodel)
        {
            DAL_AccountEntity objDAL = new DAL_AccountEntity();

            Guid id = accountdmodel.CustomerID;

            objDAL.SaveAccount(accountdmodel);

            return Redirect("~/Home");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}