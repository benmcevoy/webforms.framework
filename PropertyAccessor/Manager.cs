using System;
using System.Collections.Generic;

namespace PropertyAccessor
{
    public class Manager
    {
        private static readonly Dictionary<Type, TypeModel> _typeCache = new Dictionary<Type, TypeModel>(128);

        public static TypeModel CreateTypeModel(Type type)
        {
            if (!_typeCache.ContainsKey(type))
            {
                _typeCache[type] = new TypeModel(type);
            }

            return _typeCache[type];
        }
    }
}
