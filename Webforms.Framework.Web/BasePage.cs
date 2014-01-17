using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Webforms.Framework.Data;

namespace Webforms.Framework.Web
{
    public abstract class BasePage : Page
    {
        private readonly WebFormModelBinder _webFormModelBinder;
        private readonly ValidationRunner _validationRunner;

        public BasePage()
        {
            _webFormModelBinder = new WebFormModelBinder();
            _validationRunner = new ValidationRunner();
        }

        protected virtual void RegisterModelBinder<T>(IModelBinder<T> modelBinder) where T : new()
        {
            _webFormModelBinder.RegisterModelBinder<T>(modelBinder);
        }

        protected virtual IEnumerable<T> BindRepeater<T>(Repeater source) where T : new()
        {
            return _webFormModelBinder.BindRepeater<T>(source);
        }

        protected virtual T Bind<T>(Control source) where T : new()
        {
            return _webFormModelBinder.Bind<T>(source);
        }

        protected virtual T BindForm<T>(string modelPrefix) where T : new()
        {
            return FormModelBinder.Bind<T>(modelPrefix, this.Request.Form);
        }

        protected virtual IEnumerable<T> BindFormEnumerable<T>(string modelPrefix) where T : new()
        {
            return FormModelBinder.BindEnumerable<T>(modelPrefix, this.Request.Form);
        }

        protected virtual IEnumerable<ErrorInfo> Validate<T>(T model)
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