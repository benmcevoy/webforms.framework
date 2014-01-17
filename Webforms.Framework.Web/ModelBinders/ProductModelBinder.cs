using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Webforms.Framework.Data;
using Webforms.Framework.Web.Model;

namespace Webforms.Framework.Web.ModelBinders
{
    /// <summary>
    /// Simple custom model binder
    /// </summary>
    public class ProductModelBinder : IModelBinder<Product>
    {
        public Product Bind(Control source)
        {
            var result = new Product();

            result.Name = (source.FindControl("Name") as TextBox).Text;
            result.Quantity = Convert.ToInt32((source.FindControl("QuantityTextBox") as TextBox).Text);

            return result;
        }
    }
}