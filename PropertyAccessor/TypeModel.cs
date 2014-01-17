using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PropertyAccessor
{
    public class TypeModel
    {
        public TypeModel(Type type)
        {
            this.Type = type;
            this.Name = type.Name;
            this.PropertyDescriptions = TypeDescriptor.GetProperties(type);

            this.Properties = new Dictionary<string, PropertyModel>(32);

            foreach (PropertyDescriptor propertyDescriptor in this.PropertyDescriptions)
            {
                this.Properties[propertyDescriptor.Name] = new PropertyModel(propertyDescriptor);
            }
        }

        public string Name { get; set; }

        public Type Type { get; set; }

        public PropertyDescriptorCollection PropertyDescriptions { get; set; }

        public Dictionary<string, PropertyModel> Properties { get; set; }
    }
}
