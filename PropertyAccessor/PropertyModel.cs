using System;
using System.ComponentModel;
using System.Reflection;

namespace PropertyAccessor
{
    public class PropertyModel
    {
        private readonly MethodInfo _getDelegateReference;
        private readonly MethodInfo _setDelegateReference;

        public PropertyModel(PropertyDescriptor propertyDescriptor)
        {
            PropertyDescriptor = propertyDescriptor;
            Name = propertyDescriptor.Name;
            PropertyType = propertyDescriptor.PropertyType;
            PropertyInfo = propertyDescriptor.ComponentType.GetProperty(Name);

            if (PropertyInfo == null) return;

            _getDelegateReference = PropertyInfo.GetGetMethod();
            _setDelegateReference = PropertyInfo.GetSetMethod();
        }

        public object GetValue(object target)
        {
            if (_getDelegateReference == null)
            {
                return null;
            }

            return _getDelegateReference.Invoke(target, null);
        }

        public void SetValue(object target, object value)
        {
            if (_setDelegateReference == null)
            {
                return;
            }

            _setDelegateReference.Invoke(target, new[] { value });
        }

        public string Name { get; private set; }

        public Type PropertyType { get; private set; }

        public PropertyDescriptor PropertyDescriptor { get; private set; }

        public PropertyInfo PropertyInfo { get; private set; }
    }
}
