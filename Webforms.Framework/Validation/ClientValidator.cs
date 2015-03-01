using System;
using System.ComponentModel.DataAnnotations;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Webforms.Framework.Validation
{
    public abstract class ClientValidator
    {
        private readonly Label _content;
        private readonly DataAnnotationValidator _parentValidator;
        private readonly ValidationAttribute _validationAttribute;

        protected ClientValidator(DataAnnotationValidator parentValidator, ValidationAttribute validationAttribute, string errorMessage)
        {
            _parentValidator = parentValidator;
            _validationAttribute = validationAttribute;

            _content = new Label()
            {
                ForeColor = parentValidator.ForeColor,
                Text = string.Concat(errorMessage, " "),
                ID = GetId()
            };

            AddParentAttributes(errorMessage);

            AddValidatorAttributes();
        }

        public abstract void AddValidatorAttributes();

        private void AddParentAttributes(string errorMessage)
        {
            if (ParentValidator.ControlToValidate.Length > 0)
            {
                AddAttributesToRender("controltovalidate", GetControlRenderId(ParentValidator.ControlToValidate));
            }

            if (ParentValidator.SetFocusOnError)
            {
                AddAttributesToRender("focusOnError", "t");
            }

            if (errorMessage.Length > 0)
            {
                AddAttributesToRender("errormessage", errorMessage);
            }

            var displayMode = ParentValidator.Display;

            if (displayMode != ValidatorDisplay.Static)
            {
                AddAttributesToRender("display", PropertyConverter.EnumToString(typeof(ValidatorDisplay), displayMode));
            }

            if (!ParentValidator.IsValid)
            {
                AddAttributesToRender("isvalid", "False");
            }

            if (!ParentValidator.Enabled)
            {
                AddAttributesToRender("enabled", "False");
            }

            if (ParentValidator.ValidationGroup.Length > 0)
            {
                AddAttributesToRender("validationGroup", ParentValidator.ValidationGroup);
            }
        }

        private string GetControlRenderId(string name)
        {
            var control = ParentValidator.FindControl(name);

            if (control == null)
            {
                return string.Empty;
            }

            return control.ClientID;
        }

        private string GetId()
        {
            return string.Format("{0}_cv_{1}", ParentValidator.ID, Guid.NewGuid().ToString("D").Replace("-", ""));
        }

        public virtual void Render(HtmlTextWriter writer)
        {
            var displayMode = ParentValidator.Display;

            switch (displayMode)
            {
                case ValidatorDisplay.Dynamic:
                    if (ParentValidator.IsValidationAttributeValid(_validationAttribute))
                    {
                        _content.Style["display"] = "none";
                    }
                    break;

                case ValidatorDisplay.None:
                    _content.Style["display"] = "none";
                    break;

                case ValidatorDisplay.Static:
                    if (ParentValidator.IsValidationAttributeValid(_validationAttribute))
                    {
                        _content.Style["visibility"] = "hidden";
                    }
                    break;
            }

            _content.RenderControl(writer);
        }

        protected void AddAttributesToRender(string attributeName, string attributeValue)
        {
            ParentValidator.Page.ClientScript.RegisterExpandoAttribute(_content.ClientID, attributeName, attributeValue);
        }

        protected virtual ValidationAttribute ValidationAttribute
        {
            get { return _validationAttribute; }
        }

        protected virtual DataAnnotationValidator ParentValidator
        {
            get { return _parentValidator; }
        }

        public virtual string ClientID
        {
            get { return _content.ClientID; }
        }
    }
}
