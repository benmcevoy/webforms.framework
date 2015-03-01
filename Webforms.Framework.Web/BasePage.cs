using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Webforms.Framework.Data;
using Webforms.Framework.Validation;

namespace Webforms.Framework.Web
{
    public abstract class BasePage<TViewModel> : Page
        where TViewModel : new()
    {
        private readonly WebFormModelBinder<TViewModel> _webFormModelBinder;
        private readonly ValidationRunner _validationRunner;
        private readonly FormControlModelBinder<TViewModel> _formModelBinder;

        protected BasePage()
        {
            _webFormModelBinder = new WebFormModelBinder<TViewModel>();
            _validationRunner = new ValidationRunner();
            _formModelBinder = new FormControlModelBinder<TViewModel>();
        }

        protected virtual void SetModelBinder<T>(IControlModelBinder<TViewModel> controlModelBinder) 
        {
            _webFormModelBinder.SetModelBinder(controlModelBinder);
        }

        protected virtual IEnumerable<TViewModel> BindRepeater(Repeater source) 
        {
            return _webFormModelBinder.BindRepeater(source);
        }

        protected virtual TViewModel Bind(Control source) 
        {
            return _webFormModelBinder.Bind(source);
        }

        protected virtual TViewModel BindForm(string modelPrefix) 
        {
            return _formModelBinder.Bind(modelPrefix, this.Request.Form);
        }

        protected virtual IEnumerable<TViewModel> BindFormEnumerable(string modelPrefix) 
        {
            return _formModelBinder.BindEnumerable(modelPrefix, this.Request.Form);
        }

        protected virtual IEnumerable<ErrorInfo> Validate(TViewModel model)
        {
            return _validationRunner.Validate(model);
        }

        protected virtual void AddValidationErrors(IEnumerable<ErrorInfo> errors)
        {
            foreach (var error in errors)
            {
                AddValidationError(error.ErrorMessage);
            }
        }

        protected virtual void AddValidationError(string errorMessage)
        {
            var c = new CustomValidator() { ErrorMessage = errorMessage, IsValid = false };
            Page.Validators.Add(c);
        }

        protected virtual void AddValidationError(string errorMessage, string validationGroup)
        {
            var c = new CustomValidator() { ErrorMessage = errorMessage, IsValid = false, ValidationGroup = validationGroup };
            Page.Validators.Add(c);
        }
    }
}