using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDWebApp.Models.DAL
{
    public class DAL_AccountEntity
    {
        IOrganizationService service = null;
        public List<AccountEntityModels> RetriveRecords()
        {
            List<AccountEntityModels> info = new List<AccountEntityModels>();
            service = CRMClass.GetCrmService();
            if (service != null)
            {
                QueryExpression query = new QueryExpression
                {
                    EntityName = "rgv_customer",
                    ColumnSet = new ColumnSet("rgv_customerid", "rgv_firstname", "rgv_lastname", "rgv_emailaddress", "rgv_phonenumber")
                };

                EntityCollection accountRecord = service.RetrieveMultiple(query);
                if (accountRecord != null && accountRecord.Entities.Count > 0)
                {
                    AccountEntityModels accountModel;
                    for (int i = 0; i < accountRecord.Entities.Count; i++)
                    {
                        accountModel = new AccountEntityModels();
                        if (accountRecord[i].Contains("rgv_customerid") && accountRecord[i]["rgv_customerid"] != null)
                            accountModel.CustomerID = (Guid)accountRecord[i]["rgv_customerid"];

                        if (accountRecord[i].Contains("rgv_firstname") && accountRecord[i]["rgv_firstname"] != null)
                            accountModel.FirstName = accountRecord[i]["rgv_firstname"].ToString();

                        if (accountRecord[i].Contains("rgv_lastname") && accountRecord[i]["rgv_lastname"] != null)
                            accountModel.LastName = accountRecord[i]["rgv_lastname"].ToString();

                        if (accountRecord[i].Contains("rgv_emailaddress") && accountRecord[i]["rgv_emailaddress"] != null)
                            accountModel.EmailAddress = accountRecord[i]["rgv_emailaddress"].ToString();

                        if (accountRecord[i].Contains("rgv_phonenumber") && accountRecord[i]["rgv_phonenumber"] != null)
                            accountModel.PhoneNumber = accountRecord[i]["rgv_phonenumber"].ToString();

                        info.Add(accountModel);
                    }
                }
            }
            return info;

        }

        public AccountEntityModels getCurrentRecord(Guid customerId)
        {
            AccountEntityModels accountModel = new AccountEntityModels();
            service = CRMClass.GetCrmService();
            if (service != null)
            {
                ColumnSet cols = new ColumnSet(new String[] { "rgv_customerid", "rgv_firstname", "rgv_lastname", "rgv_emailaddress", "rgv_phonenumber" });
                Entity account = service.Retrieve("rgv_customer", customerId, cols);
                accountModel.CustomerID = customerId;
                accountModel.FirstName = account.Attributes["rgv_firstname"].ToString();
                accountModel.LastName = account.Attributes["rgv_lastname"].ToString();
                accountModel.PhoneNumber = account.Attributes["rgv_phonenumber"].ToString();
                accountModel.EmailAddress = account.Attributes["rgv_emailaddress"].ToString(); ;
            }
            return accountModel;
        }
        public void SaveAccount(AccountEntityModels objAccountModel)
        {
            service = CRMClass.GetCrmService();
            if (service != null)
            {
                Entity AccountEntity = new Entity("rgv_customer");

                if (objAccountModel.CustomerID != Guid.Empty)
                {
                    AccountEntity["rgv_customerid"] = objAccountModel.CustomerID;
                }
                AccountEntity["rgv_firstname"] = objAccountModel.FirstName;
                AccountEntity["rgv_lastname"] = objAccountModel.LastName;
                AccountEntity["rgv_emailaddress"] = objAccountModel.EmailAddress;
                AccountEntity["rgv_phonenumber"] = objAccountModel.PhoneNumber;

                if (objAccountModel.CustomerID == Guid.Empty)
                {
                    objAccountModel.CustomerID = service.Create(AccountEntity);
                }
                else
                {
                    service.Update(AccountEntity);
                }
            }
        }

    }
}