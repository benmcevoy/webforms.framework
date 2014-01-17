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

            var regularExpressionAttribute = _validationAttribute as RegularExpressionAttribute;

            if (!string.IsNullOrEmpty(regularExpressionAttribute.Pattern))
            {
                AddAttributesToRender("validationexpression", regularExpressionAttribute.Pattern);
            }
        }
    }
}
