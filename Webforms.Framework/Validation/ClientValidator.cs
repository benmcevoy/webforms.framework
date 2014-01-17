using System;
using System.ComponentModel.DataAnnotations;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Webforms.Framework.Validation
{
    public abstract class ClientValidator
    {
        private readonly Label _content;
        protected readonly DataAnnotationValidator _parentValidator;
        protected readonly ValidationAttribute _validationAttribute;

        public ClientValidator(DataAnnotationValidator parentValidator, ValidationAttribute validationAttribute, string errorMessage)
        {
            _parentValidator = parentValidator;
            _validationAttribute = validationAttribute;

            _content = new Label()
            {
                ForeColor = parentValidator.ForeColor,
                Text = string.Concat(errorMessage, " "),
                ID = GetID()
            };

            AddParentAttributes(errorMessage);

            AddValidatorAttributes();
        }

        public abstract void AddValidatorAttributes();

        private void RenderRegularExpressionAttributes()
        {
            AddAttributesToRender("evaluationfunction", "RegularExpressionValidatorEvaluateIsValid");

            var regularExpressionAttribute = _validationAttribute as RegularExpressionAttribute;

            if (!string.IsNullOrEmpty(regularExpressionAttribute.Pattern))
            {
                AddAttributesToRender("validationexpression", regularExpressionAttribute.Pattern);
            }
        }

        private void AddParentAttributes(string errorMessage)
        {
            if (_parentValidator.ControlToValidate.Length > 0)
            {
                AddAttributesToRender("controltovalidate", GetControlRenderID(_parentValidator.ControlToValidate));
            }

            if (_parentValidator.SetFocusOnError)
            {
                AddAttributesToRender("focusOnError", "t");
            }

            if (errorMessage.Length > 0)
            {
                AddAttributesToRender("errormessage", errorMessage);
            }

            ValidatorDisplay displayMode = _parentValidator.Display;

            if (displayMode != ValidatorDisplay.Static)
            {
                AddAttributesToRender("display", PropertyConverter.EnumToString(typeof(ValidatorDisplay), displayMode));
            }

            if (!_parentValidator.IsValid)
            {
                AddAttributesToRender("isvalid", "False");
            }

            if (!_parentValidator.Enabled)
            {
                AddAttributesToRender("enabled", "False");
            }

            if (_parentValidator.ValidationGroup.Length > 0)
            {
                AddAttributesToRender("validationGroup", _parentValidator.ValidationGroup);
            }
        }

        private string GetControlRenderID(string name)
        {
            Control control = _parentValidator.FindControl(name);

            if (control == null)
            {
                return string.Empty;
            }

            return control.ClientID;
        }

        private string GetID()
        {
            return string.Format("{0}_cv_{1}", _parentValidator.ID, Guid.NewGuid().ToString("D").Replace("-", ""));
        }

        public virtual void Render(HtmlTextWriter writer)
        {
            ValidatorDisplay displayMode = _parentValidator.Display;

            switch (displayMode)
            {
                case ValidatorDisplay.Dynamic:
                    if (_parentValidator.IsValidationAttributeValid(_validationAttribute))
                    {
                        _content.Style["display"] = "none";
                    }
                    break;

                case ValidatorDisplay.None:
                    _content.Style["display"] = "none";
                    break;

                case ValidatorDisplay.Static:
                    if (_parentValidator.IsValidationAttributeValid(_validationAttribute))
                    {
                        _content.Style["visibility"] = "hidden";
                    }
                    break;
                default:
                    break;
            }

            _content.RenderControl(writer);
        }

        protected void AddAttributesToRender(string attributeName, string attributeValue)
        {
            _parentValidator.Page.ClientScript.RegisterExpandoAttribute(_content.ClientID, attributeName, attributeValue);
        }

        public virtual ValidationAttribute ValidationAttribute
        {
            get { return _validationAttribute; }
        }

        public virtual string ClientID
        {
            get { return _content.ClientID; }
        }
    }
}
