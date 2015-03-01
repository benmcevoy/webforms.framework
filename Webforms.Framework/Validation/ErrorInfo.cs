namespace Webforms.Framework.Validation
{
	public class ErrorInfo
	{
		public ErrorInfo() { }

        public ErrorInfo(object instance, string propertyName, string errorMessage)
		{
            Instance = instance;
			PropertyName = propertyName;
            ErrorMessage = errorMessage;
		}

        public virtual string PropertyName { get; private set; }

        public virtual string ErrorMessage { get; private set; }

        public virtual object Instance { get; private set; }
	}
}