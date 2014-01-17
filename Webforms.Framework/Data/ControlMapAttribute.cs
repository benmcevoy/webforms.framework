using System;

namespace Webforms.Framework.Data
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ControlMapAttribute : Attribute
    {
        public ControlMapAttribute(string controlName, string propertyName)
        {
            this.ControlName = controlName;
            this.PropertyName = propertyName;
        }

        public string ControlName { get; set; }

        public string PropertyName { get; set; }
    }
}
