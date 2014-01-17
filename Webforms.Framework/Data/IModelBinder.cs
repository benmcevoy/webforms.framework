using System;
using System.Web.UI;

namespace Webforms.Framework.Data
{
    public interface IModelBinder<T> where T : new()
    {
        T Bind(Control source);
    }
}
