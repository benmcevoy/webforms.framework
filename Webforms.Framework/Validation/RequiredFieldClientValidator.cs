using System.ComponentModel.DataAnnotations;

namespace Webforms.Framework.Validation
{
    public class RequiredFieldClientValidator : ClientValidator
    {
        public RequiredFieldClientValidator(DataAnnotationValidator parentValidator, ValidationAttribute validationAttribute, string errorMessage) 
            : base(parentValidator, validationAttribute, errorMessage)
        {
        }

        public override void AddValidatorAttributes()
        {
            AddAttributesToRender("evaluationfunction", "RequiredFieldValidatorEvaluateIsValid");
            AddAttributesToRender("initialvalue", "");
        }
    }
}
