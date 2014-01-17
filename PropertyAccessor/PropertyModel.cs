using System;
using System.ComponentModel;
using System.Reflection;

namespace PropertyAccessor
{
    public class PropertyModel
    {
        private readonly DelegateReference _getDelegateReference;
        private readonly DelegateReference _setDelegateReference;

        public PropertyModel(PropertyDescriptor propertyDescriptor)
        {
            this.PropertyDescriptor = propertyDescriptor;
            this.Name = propertyDescriptor.Name;
            this.PropertyType = propertyDescriptor.PropertyType;
            this.PropertyInfo = propertyDescriptor.ComponentType.GetProperty(this.Name);

            var getPropertyInfo = propertyDescriptor.ComponentType.GetProperty(this.Name, BindingFlags.GetProperty);
            var setPropertyInfo = propertyDescriptor.ComponentType.GetProperty(this.Name, BindingFlags.SetProperty);

            if (getPropertyInfo != null)
            {
                _getDelegateReference = new DelegateReference(this.PropertyInfo, this.PropertyInfo.GetGetMethod());
            }

            if (setPropertyInfo != null)
            {
                _setDelegateReference = new DelegateReference(this.PropertyInfo, this.PropertyInfo.GetSetMethod());
            }
        }

        public string Name { get; set; }

        public Type PropertyType { get; set; }

        public PropertyDescriptor PropertyDescriptor { get; set; }

        public AttributeCollection Attributes { get; set; }

        public PropertyInfo PropertyInfo { get; set; }

        public object GetValue(object target)
        {
            if (_getDelegateReference == null)
            {
                return null;
            }

            var d = _getDelegateReference.TryGetDelegate(target);

            if (d != null)
            {
                return d.DynamicInvoke();
            }

            return null;
        }

        public void SetValue(object target, object value)
        {
            if (_setDelegateReference == null)
            {
                return;
            }

            var d = _setDelegateReference.TryGetDelegate(target);

            if (d != null)
            {
                d.DynamicInvoke();
            }
        }
    }
}
