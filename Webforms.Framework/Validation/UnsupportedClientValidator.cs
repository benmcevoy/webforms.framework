using System.ComponentModel.DataAnnotations;

namespace Webforms.Framework.Validation
{
    public class UnsupportedClientValidator : ClientValidator
    {
        public UnsupportedClientValidator(DataAnnotationValidator parentValidator, ValidationAttribute validationAttribute, string errorMessage)
            : base(parentValidator, validationAttribute, errorMessage)
        {
        }

        public override void AddValidatorAttributes()
        {
        }
    }
}
