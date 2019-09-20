using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDWebApp.Models.DAL
{
    public class DAL_Offer
    {

        IOrganizationService service = null;
        public List<Offer> RetriveRecords()
        {
            List<Offer> info = new List<Offer>();
            service = CRMClass.GetCrmService();
            if (service != null)
            {
                QueryExpression query = new QueryExpression
                {
                    EntityName = "rgv_offer",
                    ColumnSet = new ColumnSet("rgv_offerid", "rgv_offername", "rgv_menulookup")
                };

                EntityCollection offerRecord = service.RetrieveMultiple(query);
                if (offerRecord != null && offerRecord.Entities.Count > 0)
                {
                    Offer offerModel;
                    for (int i = 0; i < offerRecord.Entities.Count; i++)
                    {
                        offerModel = new Offer();
                        if (offerRecord[i].Contains("rgv_offerid") && offerRecord[i]["rgv_offerid"] != null)
                            offerModel.OfferID = (Guid)offerRecord[i]["rgv_offerid"];

                        if (offerRecord[i].Contains("rgv_offername") && offerRecord[i]["rgv_offername"] != null)
                            offerModel.OfferName = offerRecord[i]["rgv_offername"].ToString();

                        if (offerRecord[i].Contains("rgv_menulookup") && offerRecord[i]["rgv_menulookup"] != null)
                            offerModel.MenuItemName = getCurrentMenuitem(((EntityReference)offerRecord[i]["rgv_menulookup"]).Id);

                        info.Add(offerModel);
                    }
                }
            }
            return info;

        }
        public string getCurrentMenuitem(Guid MenuId)
        {
            string MenuItemName = string.Empty;
            service = CRMClass.GetCrmService();
            if (service != null)
            {

                ColumnSet cols = new ColumnSet(true);
                Entity offer = service.Retrieve("rgv_menuitem", MenuId, cols);
                if (offer.Attributes.Contains("rgv_itemname"))
                {
                    MenuItemName = offer.Attributes["rgv_itemname"].ToString();
                }

            }
            return MenuItemName;
        }


        public Offer getCurrentRecord(Guid offerId)
        {
            Offer offerModel = new Offer();
            service = CRMClass.GetCrmService();
            if (service != null)
            {
                ColumnSet cols = new ColumnSet(new String[] { "rgv_offerid", "rgv_offername", "rgv_menulookup" });
                Entity offer = service.Retrieve("rgv_offer", offerId, cols);
                offerModel.OfferID = offerId;
                offerModel.OfferName = offer.Attributes["rgv_offername"].ToString();
                if (offer.Attributes.Contains("rgv_menulookup"))
                {
                    offerModel.MenuItem = (EntityReference)offer["rgv_menulookup"];
                }
            }
            return offerModel;
        }
        public List<Microsoft.Xrm.Sdk.EntityReference> GetEntityReference()
        {
            try
            {
                List<Microsoft.Xrm.Sdk.EntityReference> info = new List<Microsoft.Xrm.Sdk.EntityReference>();
                service = CRMClass.GetCrmService();
                if (service != null)
                {
                    QueryExpression query = new QueryExpression
                    {
                        EntityName = "rgv_menuitem",
                        ColumnSet = new ColumnSet(true)
                    };
                    EntityCollection MenuItems = service.RetrieveMultiple(query);
                    if (MenuItems != null && MenuItems.Entities.Count > 0)
                    {
                        Microsoft.Xrm.Sdk.EntityReference itm;
                        for (int i = 0; i < MenuItems.Entities.Count; i++)
                        {
                            itm = new EntityReference();
                            if (MenuItems[i].Id != null)
                                itm.Id = MenuItems[i].Id;
                            if (MenuItems[i].Contains("rgv_itemname") && MenuItems[i]["rgv_itemname"] != null)
                                itm.Name = MenuItems[i]["rgv_itemname"].ToString();
                            itm.LogicalName = "rgv_menuitem";
                            info.Add(itm);
                        }
                    }
                }
                return info;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public void SaveAccount(Offer objOfferModel)
        {
            service = CRMClass.GetCrmService();
            if (service != null)
            {
                Entity OfferEntity = new Entity("rgv_offer");

                if (objOfferModel.OfferID != Guid.Empty)
                {
                    OfferEntity["rgv_offerid"] = objOfferModel.OfferID;
                }
                OfferEntity["rgv_offername"] = objOfferModel.OfferName;
                OfferEntity["rgv_menulookup"] = new Microsoft.Xrm.Sdk.EntityReference { Id = objOfferModel.MenuItem.Id, LogicalName = "rgv_menuitem" };

                if (objOfferModel.OfferID == Guid.Empty)
                {
                    objOfferModel.OfferID = service.Create(OfferEntity);
                }
                else
                {
                    service.Update(OfferEntity);
                }
            }
        }

    }
}