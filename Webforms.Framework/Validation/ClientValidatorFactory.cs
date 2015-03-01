using System.ComponentModel.DataAnnotations;

namespace Webforms.Framework.Validation
{
    public static class ClientValidatorFactory
    {
        public static ClientValidator Create(DataAnnotationValidator parentValidator, ValidationAttribute validationAttribute, string errorMessage)
        {
            
                if (validationAttribute is RequiredAttribute)
                {
                    return new RequiredFieldClientValidator(parentValidator, validationAttribute, errorMessage);
                }

                if (validationAttribute is RangeAttribute)
                {
                    return new RangeFieldClientValidator(parentValidator, validationAttribute, errorMessage);
                }

                if (validationAttribute is RegularExpressionAttribute)
                {
                    return new RegularExpressionFieldClientValidator(parentValidator, validationAttribute, errorMessage);
                }

                return new UnsupportedClientValidator(parentValidator, validationAttribute, errorMessage);
        }
    }
}
