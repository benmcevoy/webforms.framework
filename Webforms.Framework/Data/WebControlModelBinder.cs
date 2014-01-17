using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using PropertyAccessor;

namespace Webforms.Framework.Data
{
    public sealed class WebControlModelBinder
    {
        private static readonly Dictionary<string, PropertyModel> _controlPropertyCache = new Dictionary<string, PropertyModel>(32);
        private static readonly Dictionary<Type, ControlMapAttribute> _controlMapAttributeCache = new Dictionary<Type, ControlMapAttribute>(32);
        private static readonly Dictionary<string, Type> _candidateProperties = new Dictionary<string, Type>(5)
            {
                { "SelectedValue", typeof(string) },
                { "SelectedDate", typeof(DateTime) },
                { "Checked", typeof(bool) },
                { "Value", typeof(string) },
                { "Text", typeof(string) },
            };

        public WebControlModelBinder() { }

        public object Bind<T>(Control source) where T : new()
        {
            var result = new T();
            var properties = GetProperties(typeof(T));

            foreach (PropertyModel property in properties.Values)
            {
                if (TrySetFromControlMap(source, result, property))
                {
                    continue;
                }

                foreach (Control control in source.Controls)
                {
                    if (control.ID == property.Name)
                    {
                        if (TryFindAndSetObjectProperty(control, result, property))
                        {
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private bool TrySetFromControlMap<T>(Control source, T result, PropertyModel property)
        {
            var controlMap = GetControlMapAttribute(typeof(T));

            if (controlMap != null)
            {
                if (controlMap is ControlMapIgnoreAttribute)
                {
                    return true;
                }

                if (controlMap is ControlMapAttribute)
                {
                    var map = controlMap as ControlMapAttribute;
                    var mappedControl = source.FindControl(map.ControlName);

                    if (mappedControl != null)
                    {
                        var mappedProperty = GetProperties(mappedControl.GetType())[map.PropertyName];

                        property.SetValue(result, Convert.ChangeType(mappedProperty.GetValue(mappedControl), property.PropertyType));

                        return true;
                    }
                }
            }

            return false;
        }

        private bool TryFindAndSetObjectProperty<D>(Control source, D destination, PropertyModel destinationProperty)
        {
            if (_controlPropertyCache.ContainsKey(source.ID))
            {
                destinationProperty.SetValue(destination,
                    Convert.ChangeType(_controlPropertyCache[source.ID].GetValue(source), destinationProperty.PropertyType));
                return true;
            }

            var properties = GetProperties(source.GetType());

            foreach (var candidate in _candidateProperties)
            {
                if(!properties.ContainsKey(candidate.Key))
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

                        _controlPropertyCache[source.ID] = property;

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
            return Manager.CreateTypeModel(type).Properties;
        }

        private ControlMapAttribute GetControlMapAttribute(Type type)
        {
            if (!_controlMapAttributeCache.ContainsKey(type))
            {
                _controlMapAttributeCache[type] = TypeDescriptor.GetAttributes(type)[typeof(ControlMapAttribute)] as ControlMapAttribute;
            }

            return _controlMapAttributeCache[type];
        }
    }
}
