using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDWebApp.Models
{
    public class Offer
    {

        public Guid OfferID { get; set; }
        public string OfferName { get; set; }

        public string MenuItemName { get; set; }
        public EntityReference MenuItem { get; set; }

    }
}