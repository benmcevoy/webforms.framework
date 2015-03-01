using System.Web.UI;

namespace Webforms.Framework.Data
{
    public interface IControlModelBinder<T> 
        where T : new()
    {
        T Bind(Control source);
    }
}
