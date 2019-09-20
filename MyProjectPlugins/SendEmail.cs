using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.Crm.Sdk.Messages;
namespace onOfferCreate
{
    public class SendEmail : IPlugin
    {
        private IOrganizationService service;
        private IOrganizationServiceFactory serviceFactory;
        public void Execute(IServiceProvider serviceProvider)
        {

            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            service = serviceFactory.CreateOrganizationService(context.UserId);
            try
            {
                Entity TargetEntity = context.InputParameters["Target"] as Entity;
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {

                    //using query expression to retrieve data
                    QueryExpression query = new QueryExpression("rgv_customer");
                    query.ColumnSet = new ColumnSet(new String[] { "rgv_emailaddress" });
                    EntityCollection custCollection = service.RetrieveMultiple(query);

                    foreach (Entity item in custCollection.Entities)
                    {
                        Entity fromActivityParty = new Entity("activityparty");
                        Entity toActivityParty = new Entity("activityparty");

                        fromActivityParty["partyid"] = new EntityReference("systemuser", new Guid("b58d8ca5-b9c2-4ebc-b05c-92978b558276"));
                        toActivityParty["partyid"] = new EntityReference("rgv_customer", item.Id);

                        Entity email = new Entity("email");
                        email["from"] = new Entity[] { fromActivityParty };
                        email["to"] = new Entity[] { toActivityParty };
                        email["regardingobjectid"] = new EntityReference("rgv_customer", item.Id);
                        email["subject"] = "Offer from Spice Of India";


                        string offerContentName = string.Empty;

                        if (TargetEntity.Attributes.Contains("rgv_offername") && TargetEntity.Attributes.Contains("rgv_menulookup"))
                        {
                            Entity Menu = service.Retrieve(((EntityReference)TargetEntity.Attributes["rgv_menulookup"]).LogicalName, ((EntityReference)TargetEntity.Attributes["rgv_menulookup"]).Id, new ColumnSet(true));
                            if (Menu.Attributes.Count > 0 && Menu.Attributes.Contains("rgv_name"))
                            {
                                offerContentName = TargetEntity.Attributes["rgv_offername"].ToString() + " " + Menu.Attributes["rgv_name"].ToString();
                                email["description"] = "Hi,\n\n" + "We have a new offer. Please redeem this offer. The offer is : " + offerContentName + ". See You Soon !\n\n" + "Spice Of India Restaurant";

                            }
                        }
                        email["directioncode"] = true;
                        Guid emailId = service.Create(email);

                        // Use the SendEmail message to send an e-mail message.
                        SendEmailRequest sendEmailRequest = new SendEmailRequest
                        {
                            EmailId = emailId,
                            TrackingToken = "",
                            IssueSend = true
                        };

                        SendEmailResponse sendEmailresp = (SendEmailResponse)service.Execute(sendEmailRequest);
                    }
                }
            }
            catch (InvalidPluginExecutionException ex)
            {
                throw ex;
            }
        }
    }
}
