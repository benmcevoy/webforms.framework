using System;

namespace Webforms.Framework.Data
{
    /// <summary>
    /// Map the property that supplies a vlaue for a control
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ControlMapAttribute : Attribute
    {
        public ControlMapAttribute(string controlId, string propertyName)
        {
            ControlId = controlId;
            PropertyName = propertyName;
        }

        public string ControlId { get; private set; }

        public string PropertyName { get; private set; }
    }
}
