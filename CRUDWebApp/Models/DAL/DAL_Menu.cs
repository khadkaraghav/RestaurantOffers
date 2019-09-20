using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDWebApp.Models.DAL
{
    public class DAL_Menu
    {

        IOrganizationService service = null;
        public List<Menu> RetriveRecords()
        {
            List<Menu> info = new List<Menu>();
            service = CRMClass.GetCrmService();
            if (service != null)
            {
                QueryExpression query = new QueryExpression
                {
                    EntityName = "rgv_menuitem",
                    ColumnSet = new ColumnSet("rgv_menuitemid", "rgv_itemname", "rgv_itemprice")
                };

                EntityCollection itemRecord = service.RetrieveMultiple(query);
                if (itemRecord != null && itemRecord.Entities.Count > 0)
                {
                    Menu itemModel;
                    for (int i = 0; i < itemRecord.Entities.Count; i++)
                    {
                        itemModel = new Menu();
                        if (itemRecord[i].Contains("rgv_menuitemid") && itemRecord[i]["rgv_menuitemid"] != null)
                            itemModel.ItemID = (Guid)itemRecord[i]["rgv_menuitemid"];

                        if (itemRecord[i].Contains("rgv_itemname") && itemRecord[i]["rgv_itemname"] != null)
                            itemModel.ItemName = itemRecord[i]["rgv_itemname"].ToString();

                        if (itemRecord[i].Contains("rgv_itemprice") && itemRecord[i]["rgv_itemprice"] != null)
                            itemModel.ItemPrice = (decimal)itemRecord[i]["rgv_itemprice"];

                        info.Add(itemModel);
                    }
                }
            }
            return info;

        }

        public Menu getCurrentRecord(Guid itemId)
        {
            Menu itemModel = new Menu();
            service = CRMClass.GetCrmService();
            if (service != null)
            {
                ColumnSet cols = new ColumnSet(new String[] { "rgv_menuitemid", "rgv_itemname", "rgv_itemprice" });
                Entity item = service.Retrieve("rgv_menuitem", itemId, cols);
                itemModel.ItemID = itemId;
                itemModel.ItemName = item.Attributes["rgv_itemname"].ToString();
                itemModel.ItemPrice = (decimal)item.Attributes["rgv_itemprice"];


            }
            return itemModel;
        }



        public void SaveAccount(Menu objItemModel)
        {
            service = CRMClass.GetCrmService();
            if (service != null)
            {
                Entity ItemEntity = new Entity("rgv_menuitem");

                if (objItemModel.ItemID != Guid.Empty)
                {
                    ItemEntity["rgv_menuitemid"] = objItemModel.ItemID;
                }
                ItemEntity["rgv_itemname"] = objItemModel.ItemName;

                ItemEntity["rgv_itemprice"] = objItemModel.ItemPrice;


                if (objItemModel.ItemID == Guid.Empty)
                {
                    objItemModel.ItemID = service.Create(ItemEntity);
                }
                else
                {
                    service.Update(ItemEntity);
                }
            }
        }

    }
}
