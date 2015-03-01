using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using PropertyAccessor;

namespace Webforms.Framework.Data
{
    public sealed class WebControlModelBinder<T> : IControlModelBinder<T>
        where T : new()
    {
        private readonly PropertyAccessorManager _propertyCachePropertyAccessorManager;

        // static fields in generic type means one instance per <T>
        // ReSharper disable StaticFieldInGenericType
        private static readonly Dictionary<string, PropertyModel> ControlPropertyCache = new Dictionary<string, PropertyModel>(32);
        private static readonly Dictionary<Type, ControlMapAttribute> ControlMapAttributeCache = new Dictionary<Type, ControlMapAttribute>(32);
        private static readonly Dictionary<string, Type> CandidateProperties = new Dictionary<string, Type>(5)
        // ReSharper restore StaticFieldInGenericType
            {
                { "SelectedValue", typeof(string) },
                { "SelectedDate", typeof(DateTime) },
                { "Checked", typeof(bool) },
                { "Value", typeof(string) },
                { "Text", typeof(string) },
            };

        public WebControlModelBinder(PropertyAccessorManager propertyCachePropertyAccessorManager)
        {
            _propertyCachePropertyAccessorManager = propertyCachePropertyAccessorManager;
        }

        public T Bind(Control source)
        {
            var result = new T();
            var properties = GetProperties(typeof(T));

            foreach (var property in properties.Values)
            {
                if (TrySetFromControlMap(source, result, property))
                {
                    continue;
                }

                foreach (Control control in source.Controls)
                {
                    if (control.ID != property.Name) continue;

                    if (TryFindAndSetObjectProperty(control, result, property))
                    {
                        break;
                    }
                }
            }

            return result;
        }

        private bool TrySetFromControlMap(Control source, T result, PropertyModel property)
        {
            var controlMap = GetControlMapAttribute(typeof(T));

            if (controlMap == null) return false;
            if (controlMap is ControlMapIgnoreAttribute) return true;

            var map = controlMap;
            var mappedControl = source.FindControl(map.ControlId);

            if (mappedControl == null) return false;

            var mappedProperty = GetProperties(mappedControl.GetType())[map.PropertyName];

            property.SetValue(result, Convert.ChangeType(mappedProperty.GetValue(mappedControl), property.PropertyType));

            return true;
        }

        private bool TryFindAndSetObjectProperty<TDestination>(Control source, TDestination destination, PropertyModel destinationProperty)
        {
            if (ControlPropertyCache.ContainsKey(source.ID))
            {
                destinationProperty.SetValue(destination,
                    Convert.ChangeType(ControlPropertyCache[source.ID].GetValue(source), destinationProperty.PropertyType));
                return true;
            }

            var properties = GetProperties(source.GetType());

            foreach (var candidate in CandidateProperties)
            {
                if (!properties.ContainsKey(candidate.Key))
                {
                    continue;
                }

                var property = properties[candidate.Key];

                if (property.Name == candidate.Key && property.PropertyType == candidate.Value)
                {
                    try
                    {
                        destinationProperty.SetValue(destination,
                            Convert.ChangeType(property.GetValue(source), destinationProperty.PropertyType));

                        ControlPropertyCache[source.ID] = property;

                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        private Dictionary<string, PropertyModel> GetProperties(Type type)
        {
            return _propertyCachePropertyAccessorManager.CreateTypeModel(type).Properties;
        }

        private static ControlMapAttribute GetControlMapAttribute(Type type)
        {
            if (!ControlMapAttributeCache.ContainsKey(type))
            {
                ControlMapAttributeCache[type] = TypeDescriptor.GetAttributes(type)[typeof(ControlMapAttribute)] as ControlMapAttribute;
            }

            return ControlMapAttributeCache[type];
        }
    }
}
