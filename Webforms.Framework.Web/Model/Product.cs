using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Webforms.Framework.Web.Validators;
using Webforms.Framework.Data;

namespace Webforms.Framework.Web.Model
{
    [ProductValidator]
    public class Product
    {
        [Required]
        [Range(3, 6, ErrorMessage = "Quantity must be between three and six")]
        [ControlMap("QuantityTextBox", "Text")]
        public int Quantity { get; set; }

        [Required]
        [Range(10d, 100d)]
        [ControlMapIgnore]
        public decimal Price { get; set; }
        
        //[Required(ErrorMessageResourceName = "ProductNameValidationMessage", ErrorMessageResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "ProductNameRequired", ErrorMessageResourceType = typeof(ErrorMessages))]
        //[Required()]
        [StringLength(10)]
        [Range(10, 100)]
        //[RegularExpression(@"^testme$")]
        [DisplayName("Fancy Name")]
        [ControlMap("Name", "Text")]
        public string Name { get; set; }

        [Required]
        [ControlMapIgnore]
        public DateTime CreatedDate { get; set; }
    }

    public class ErrorMessages
    {
        // must be static
        public static string ProductNameRequired { get { return "Localized error message"; } }
    }
}