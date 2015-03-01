using System;
using System.Collections.Generic;

namespace PropertyAccessor
{
    public class PropertyAccessorManager
    {
        private static readonly Dictionary<Type, TypeModel> TypeCache = new Dictionary<Type, TypeModel>(512);

        public TypeModel CreateTypeModel(object instance)
        {
            return CreateTypeModel(instance.GetType());
        }

        public TypeModel CreateTypeModel(Type type)
        {
            if (!TypeCache.ContainsKey(type))
            {
                TypeCache[type] = new TypeModel(type);
            }

            return TypeCache[type];
        }

        private static readonly PropertyAccessorManager _instance = new PropertyAccessorManager();
        public static PropertyAccessorManager Instance
        {
            get { return _instance; }
        }
    }
}
