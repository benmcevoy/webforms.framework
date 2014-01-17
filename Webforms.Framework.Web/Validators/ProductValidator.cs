using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Webforms.Framework.Web.Validators
{
    public class ProductValidator : ValidationAttribute
    {
        public ProductValidator()
        {
            this.ErrorMessage = "this is an example of a class level validator";
        }

        public override bool IsValid(object value)
        {
            return false;
        }
    }
}