using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Webforms.Framework.Data
{
    /// <summary>
    /// Bind Web Form Controls to objects
    /// </summary>
    public class WebFormModelBinder 
    {
        private readonly Dictionary<Type, object> _modelBinders;
        private readonly WebControlModelBinder _defaultModelBinder;

        /// <summary>
        /// Bind Web Form Controls to objects
        /// </summary>
        /// <remarks>
        /// This is intended to bind simple types, such as DTOs or ViewModels from values found in a collection of WebControls.
        /// There is no support for complex types, object heirarchies or even Enumerables.
        /// However if you are keen you can implement IModelBinder and write your own:)
        /// 
        /// The conventions:
        /// 
        /// Model must have a default constructor.
        /// Control ID must equal the Model Property Name.
        /// Names and IDs are case sensitive.
        /// </remarks>
        public WebFormModelBinder()
        {
            _modelBinders = new Dictionary<Type, object>();
            _defaultModelBinder = new WebControlModelBinder();
        }

        public virtual void RegisterModelBinder<T>(IModelBinder<T> modelBinder) where T : new()
        {
            _modelBinders[typeof(T)] = modelBinder;
        }

        /// <summary>
        /// Bind a Repeater to an enumerable of T
        /// </summary>
        /// <param name="repeater">An ASP.NET Repeater control</param>
        /// <returns>an enumerable of T</returns>
        /// <remarks>
        /// Within an ItemTemplate ensure any controls you wish to bind are given an ID that matches a property Name on the model.
        /// e.g.
        /// &lt;ItemTemplate&gt;
        ///    &lt;asp:TextBox runat='server' Id='Name' /&gt;
        /// &lt;/ItemTemplate&gt;
        /// 
        /// This will bind to a property called Name on the instance of T
        /// 
        /// The conventions:
        /// 
        /// Model must have a default constructor.
        /// Control ID must equal the Model Property Name.
        /// Names and IDs are case sensitive.
        /// </remarks>
        public virtual IEnumerable<T> BindRepeater<T>(Repeater repeater) where T : new()
        {
            var results = new List<T>();

            foreach (RepeaterItem item in repeater.Items)
            {
                results.Add(Bind<T>(item));
            }

            return results;
        }

        /// <summary>
        /// Bind a Control's collection of child Controls to an instance of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">An ASP.NET WebControl with child controls, e.g. Form or Panel</param>
        /// <returns>an instance of T</returns>
        public virtual T Bind<T>(Control source) where T : new()
        {
            if (_modelBinders.ContainsKey(typeof(T)))
            {
                return (T)((IModelBinder<T>)_modelBinders[typeof(T)]).Bind(source);
            }

            return (T)_defaultModelBinder.Bind<T>(source);
        }
    }
}