using System;

namespace Webforms.Framework.Data
{
	public class ErrorInfo
	{
		public ErrorInfo() { }

        public ErrorInfo(object instance, string propertyName, string errorMessage)
		{
            this.Instance = instance;
			this.PropertyName = propertyName;
            this.ErrorMessage = errorMessage;
		}

        public virtual string PropertyName { get; private set; }

        public virtual string ErrorMessage { get; private set; }

        public virtual object Instance { get; private set; }
	}
}