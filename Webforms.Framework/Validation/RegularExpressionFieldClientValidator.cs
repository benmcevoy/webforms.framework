using System;
using System.ComponentModel.DataAnnotations;

namespace Webforms.Framework.Validation
{
    public class RegularExpressionFieldClientValidator : ClientValidator
    {
        public RegularExpressionFieldClientValidator(DataAnnotationValidator parentValidator, ValidationAttribute validationAttribute, string errorMessage)
            : base(parentValidator, validationAttribute, errorMessage)
        {
        }

        public override void AddValidatorAttributes()
        {
            AddAttributesToRender("evaluationfunction", "RegularExpressionValidatorEvaluateIsValid");

            var regularExpressionAttribute = ValidationAttribute as RegularExpressionAttribute;

            if (regularExpressionAttribute == null) throw new NullReferenceException("regularExpressionAttribute");

            if (!string.IsNullOrEmpty(regularExpressionAttribute.Pattern))
            {
                AddAttributesToRender("validationexpression", regularExpressionAttribute.Pattern);
            }
        }
    }
}
