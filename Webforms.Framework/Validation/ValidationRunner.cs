using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Webforms.Framework.Validation;

namespace Webforms.Framework.Data
{
    /// <summary>
    ///  Validate an object by evaluating DataAnnotations attributes and returning any errors
    /// </summary>
    public class ValidationRunner
    {
        // refer: http://blog.stevensanderson.com/2009/01/10/xval-a-validation-framework-for-aspnet-mvc/
        
        /// <summary>
        /// Validate an object by evaluating DataAnnotations attributes and returning any errors
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>
        /// This supports class level validation. 
        /// You could implement business rules as ValidaitonAttributes
        /// </remarks>
        public virtual IEnumerable<ErrorInfo> Validate(object model)
        {
            // class level
            var modelErrors =
                    from attribute in TypeDescriptor.GetAttributes(model).OfType<ValidationAttribute>()
                    where !attribute.IsValid(model)
                    select new ErrorInfo(model, null, attribute.FormatErrorMessage(string.Empty));

            // property level
            var propertyErrors =
                    from property in TypeDescriptor.GetProperties(model).Cast<PropertyDescriptor>()
                    from attribute in property.Attributes.OfType<ValidationAttribute>()
                    where !attribute.IsValid(property.GetValue(model))
                    select new ErrorInfo(model, property.Name, attribute.FormatErrorMessage(property.DisplayName));

            return modelErrors.Union(propertyErrors);
        }
    }
}
