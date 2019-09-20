using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace CRUDWebApp.Models
{
    public class EmailAnnotation : RegularExpressionAttribute
    {
        static EmailAnnotation()
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(EmailAnnotation), typeof(RegularExpressionAttributeAdapter));
        }

        public EmailAnnotation()
            : base(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                + "@"
                + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$")
        { }

        public override string FormatErrorMessage(string name)
        {
            return "E-mail is not valid";
        }
    }
}