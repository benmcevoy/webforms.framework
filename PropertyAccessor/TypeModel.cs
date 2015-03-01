using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PropertyAccessor
{
    public class TypeModel
    {
        public TypeModel(Type type)
        {
            Type = type;
            Name = type.Name;
            FullName = type.FullName;
            PropertyDescriptions = TypeDescriptor.GetProperties(type);

            Properties = new Dictionary<string, PropertyModel>(32);

            foreach (PropertyDescriptor propertyDescriptor in PropertyDescriptions)
            {
                Properties[propertyDescriptor.Name] = new PropertyModel(propertyDescriptor);
            }
        }

        public string Name { get; private set; }

        public string FullName { get; private set; }

        public Type Type { get; private set; }

        public PropertyDescriptorCollection PropertyDescriptions { get; private set; }

        public Dictionary<string, PropertyModel> Properties { get; private set; }
    }
}
