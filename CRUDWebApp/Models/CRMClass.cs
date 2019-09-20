using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using System.Web;

namespace CRUDWebApp.Models
{
    public class CRMClass
    {
        public static IOrganizationService GetCrmService()
        {
            //AddListMembersListRequest req = new AddListMembersListRequest();
            IOrganizationService organizationService = null;
            ClientCredentials clientCredentials = new ClientCredentials();
            clientCredentials.UserName.UserName = "raghav.khadka@hlol.onmicrosoft.com";
            clientCredentials.UserName.Password = "Youtube.year.2020";

            // For Dynamics 365 Customer Engagement V9.X, set Security Protocol as TLS12
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // Get the URL from CRM, Navigate to Settings -> Customizations -> Developer Resources
            // Copy and Paste Organization Service Endpoint Address URL
            string url = "https://hlol.api.crm.dynamics.com/XRMServices/2011/Organization.svc";
            organizationService = (IOrganizationService)new OrganizationServiceProxy(new Uri(url),
             null, clientCredentials, null);

            return organizationService;
        }
    }
}