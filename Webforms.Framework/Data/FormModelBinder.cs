using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;

namespace Webforms.Framework.Data
{
    /// <summary>
    /// Bind POSTed form values to objects
    /// </summary>
    public static class FormModelBinder
    {
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
        public static IEnumerable<T> BindEnumerable<T>(string prefix, NameValueCollection form)
           where T : new()
        {
            var results = new List<T>();
            var properties = new T().GetType().GetProperties();
            int index = -1;
            var indexRegex = new Regex(string.Format(@"^{0}\[(\d?)\].*$", prefix));

            Array.Sort(form.AllKeys);

            foreach (var key in form.AllKeys)
            {
                // already matched?
                if (key.StartsWith(string.Format("{0}[{1}]", prefix, index)))
                {
                    continue;
                }

                if (key.StartsWith(prefix))
                {
                    var item = new T();
                    var matches = indexRegex.Match(key);

                    if (matches.Success && matches.Groups.Count == 2)
                    {
                        if (Int32.TryParse(matches.Groups[1].Captures[0].Value, out index))
                        {
                            foreach (var property in properties)
                            {
                                var propertyKey = string.Format("{0}[{1}].{2}", prefix, index, property.Name);

                                object result;

                                if (form.AllKeys.Contains(propertyKey))
                                {
                                    if (TryConvertToType(form[propertyKey], property.PropertyType, out result))
                                    {
                                        property.SetValue(item, result, null);
                                    }
                                }
                            }

                            results.Add(item);
                        }
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Bind POSTed form values to a single object
        /// </summary>
        /// <param name="form">A POSTed form, e.g. Request.Form</param>
        /// <param name="prefix">Class name prefix, e.g. Product</param>
        /// <returns></returns>
        public static T Bind<T>(string prefix, NameValueCollection form) where T : new()
        {
            var properties = new T().GetType().GetProperties();
            var item = new T();

            foreach (var property in properties)
            {
                var propertyKey = string.Format("{0}.{1}", prefix, property.Name);
                object result;

                if (form.AllKeys.Contains(propertyKey))
                {
                    if (TryConvertToType(form[propertyKey], property.PropertyType, out result))
                    {
                        property.SetValue(item, result, null);
                    }
                }
            }

            return item;
        }

        private static bool TryConvertToType(string value, Type type, out object result)
        {
            result = value;

            try
            {
                switch (type.Name)
                {
                    case "String":
                        return true;

                    case "Int32":
                        result = Convert.ToInt32(value);
                        return true;

                    case "Boolean":
                        result = value == "on" ? true : false;
                        return true;

                    case "DateTime":
                        result = string.IsNullOrEmpty(value) ? new DateTime() : Convert.ToDateTime(value);
                        return true;

                    case "Double":
                        result = Convert.ToDouble(value);
                        return true;

                    case "Decimal":
                        result = Convert.ToDecimal(value);
                        return true;
                }
            }
            catch { }

            return false;
        }
    }
}