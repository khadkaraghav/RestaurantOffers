using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRUDWebApp.Models
{
    public class Menu
    {

        public Guid ItemID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string ItemName { get; set; }


        [Required]
        public decimal ItemPrice { get; set; }

        

    }
}