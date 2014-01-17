using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Webforms.Framework.Validation
{
    /// <summary>
    /// Data Annotation validator. Use at your own peril.
    /// </summary>
    /// <remarks>
    /// http://www.codeproject.com/Articles/95158/Building-ASP-NET-Validator-using-Data-Annotations
    /// </remarks>
    [ToolboxData("<{0}:DataAnnotationValidator runat=\"server\"></{0}:DataAnnotationValidator>")]
    public class DataAnnotationValidator : BaseValidator
    {
        private IEnumerable<ValidationAttribute> _validationAttributes;
        private Type _source;
        private List<ClientValidator> _clientValidators;

        public DataAnnotationValidator()
            : base()
        {
            this.ErrorMessage = "The field is invalid.";
            _validationAttributes = Enumerable.Empty<ValidationAttribute>();
            _clientValidators = new List<ClientValidator>();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            SetValidationAttributes();

            var value = GetControlValidationValue(ControlToValidate);

            _clientValidators = new List<ClientValidator>();

            foreach (var attribute in _validationAttributes)
            {
                _clientValidators.Add(ClientValidatorFactory.Create(this, attribute, attribute.FormatErrorMessage(_property.DisplayName)));
            }

            base.RegisterValidatorCommonScript();
        }

        protected override void RegisterValidatorDeclaration()
        {
            var validators = new StringBuilder();
            var seperator = "";

            foreach (var clientValidator in _clientValidators)
            {
                validators.AppendFormat("{0}document.getElementById(\"{1}\")", seperator, clientValidator.ClientID);
                seperator = ", ";
            }

            Page.ClientScript.RegisterArrayDeclaration("Page_Validators", validators.ToString());
        }

        protected override void Render(HtmlTextWriter writer)
        {
            foreach (var clientValidator in _clientValidators)
            {
                clientValidator.Render(writer);
            }

            RegisterValidatorDeclaration();
        }

        private void SetValidationAttributes()
        {
            _source = GetValidatedType();
            _property = GetValidatedProperty(_source);
            _validationAttributes = _property.Attributes.OfType<ValidationAttribute>();
        }

        protected override bool EvaluateIsValid()
        {
            SetValidationAttributes();

            var value = GetControlValidationValue(ControlToValidate);

            foreach (var attribute in _validationAttributes)
            {
                // TODO: need type checking?  int range blew up on string input

                if (!attribute.IsValid(value))
                {
                    this.ErrorMessage = attribute.FormatErrorMessage(_property.DisplayName);
                    return false;
                }
            }

            return true;
        }

        internal bool IsValidationAttributeValid(ValidationAttribute validationAttribute)
        {
            var value = GetControlValidationValue(ControlToValidate);

            try
            {
                // TODO: needs type coercion
                return validationAttribute.IsValid(value);
            }
            catch
            {
                return false;
            }
        }

        private Type GetValidatedType()
        {
            if (string.IsNullOrEmpty(Type))
            {
                throw new InvalidOperationException("Null Type can't be validated");
            }

            var validatedType = System.Type.GetType(Type);

            if (validatedType == null)
            {
                throw new InvalidOperationException(string.Format("{0}:{1}", "Invalid Type", Type));
            }

            return validatedType;
        }

        private PropertyDescriptor GetValidatedProperty(Type source)
        {
            var property = TypeDescriptor.GetProperties(source)
                .Cast<PropertyDescriptor>()
                .FirstOrDefault(p => p.Name == this.Property);

            if (property == null)
            {
                throw new InvalidOperationException(string.Format("{0}:{1}", "Validated property does not exist", this.Property));
            }

            return property;
        }

        private PropertyDescriptor _property;
        internal PropertyDescriptor PropertyDescriptor
        {
            get { return _property; }
        }

        /// <summary>
        /// The type of the source to check as TypeName, Assembly
        /// </summary>
        public virtual string Type { get; set; }

        /// <summary>
        /// The property that is annotated
        /// </summary>
        public virtual string Property { get; set; }
    }
}
