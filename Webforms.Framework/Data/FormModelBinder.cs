using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using PropertyAccessor;

namespace Webforms.Framework.Data
{
    /// <summary>
    /// Bind POSTed form values to objects
    /// </summary>
    public sealed class FormControlModelBinder<T>
        where T : new()
    {
        private readonly PropertyAccessorManager _propertyAccessorManager;

        public FormControlModelBinder()
            : this(PropertyAccessorManager.Instance)
        {

        }

        public FormControlModelBinder(PropertyAccessorManager propertyAccessorManager)
        {
            _propertyAccessorManager = propertyAccessorManager;
        }

        /// <summary>
        /// Bind POSTed form values to objects
        /// </summary>
        /// <remarks>
        /// This is intended to bind simple types, such as DTOs or ViewModels from values found in a NameValueCollection (form POST).
        /// There is no support for complex types, object heirarchies or even Enumerables.
        /// 
        /// The conventions:
        /// 
        /// Model must have a default constructor.
        /// Form Name must equal the Model Property Name.
        /// Names and are case sensitive.
        /// For collections Form names must be in the form ClassName[index].PropertyName
        /// which is similar to ASP.NET MVC
        /// 
        /// e.g.
        /// &lt;input name='Product[0].Name' value='Product 1 name' /gt;
        /// &lt;input name='Product[0].Price' value='Product 1 Price' /gt;
        /// &lt;input name='Product[1].Name' value='Product 2 name' /gt;
        /// 
        /// and so on 
        /// </remarks>
        /// <param name="form">A POSTed form, e.g. Request.Form</param>
        /// <param name="prefix">Class name prefix, e.g. Product</param>
        public IEnumerable<T> BindEnumerable(string prefix, NameValueCollection form)
        {
            var results = new List<T>();
            var properties = _propertyAccessorManager.CreateTypeModel(typeof(T)).Properties;
            var index = -1;
            var indexRegex = new Regex(string.Format(@"^{0}\[(\d?)\].*$", prefix));

            Array.Sort(form.AllKeys);

            foreach (var key in form.AllKeys)
            {
                if (!key.StartsWith(prefix)) continue;

                // already matched?
                if (key.StartsWith(string.Format("{0}[{1}]", prefix, index)))
                {
                    continue;
                }

                var item = new T();
                var matches = indexRegex.Match(key);

                if (!matches.Success || matches.Groups.Count != 2) continue;

                if (!Int32.TryParse(matches.Groups[1].Captures[0].Value, out index)) continue;

                foreach (var property in properties)
                {
                    var propertyKey = string.Format("{0}[{1}].{2}", prefix, index, property.Key);

                    if (!form.AllKeys.Contains(propertyKey)) continue;

                    object result;

                    if (TryConvertToType(form[propertyKey], property.Value.PropertyType, out result))
                    {
                        property.Value.SetValue(item, result);
                    }
                }

                results.Add(item);
            }

            return results;
        }

        /// <summary>
        /// Bind POSTed form values to a single object
        /// </summary>
        /// <param name="form">A POSTed form, e.g. Request.Form</param>
        /// <param name="prefix">Class name prefix, e.g. Product</param>
        /// <returns></returns>
        public T Bind(string prefix, NameValueCollection form) 
        {
            var item = new T();
            var properties = _propertyAccessorManager.CreateTypeModel(item.GetType()).Properties;

            foreach (var property in properties)
            {
                var propertyKey = string.Format("{0}.{1}", prefix, property.Key);

                if (!form.AllKeys.Contains(propertyKey)) continue;

                object result;

                if (TryConvertToType(form[propertyKey], property.Value.PropertyType, out result))
                {
                    property.Value.SetValue(item, result);
                }
            }

            return item;
        }

        private static bool TryConvertToType(string value, Type type, out object result)
        {
            result = value;

            try
            {
                result = Convert.ChangeType(value, type);
            }
            catch { }

            return false;
        }
    }
}