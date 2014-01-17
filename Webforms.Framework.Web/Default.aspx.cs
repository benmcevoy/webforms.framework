using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Webforms.Framework.Web.Model;
using Webforms.Framework.Web.ModelBinders;
using System.Diagnostics;

namespace Webforms.Framework.Web
{
    public partial class Default : BasePage
    {
        public Default()
        {
            // uncomment to use the custom model binder
            // RegisterModelBinder<Product>(new ProductModelBinder());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Bind();
        }

        private void Bind()
        {
            var products = new List<Product>();

            products.Add(new Product() { Name = "Product 1" });
            //products.Add(new Product() { Name = "Product 2" });
            //products.Add(new Product() { Name = "Product 3" });

            ProductRepeater.DataSource = products;
            ProductRepeater.DataBind();
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            var sw = Stopwatch.StartNew();

            var products = BindRepeater<Product>(ProductRepeater);

            sw.Stop();

            Debug.WriteLine(sw.Elapsed.Ticks);

            foreach (var product in products)
            {
                var errors = Validate<Product>(product);

                AddValidationErrors(errors);
            }
        }

        // Helper method
        protected Product AsProduct(RepeaterItem item)
        {
            return item.DataItem as Product;
        }


        public T As<T>(RepeaterItem item) where T : class
        {
            return item.DataItem as T;
        }
    }
}