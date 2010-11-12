using System;
using System.Collections;
using System.Linq;
using System.Web.Mvc;

namespace FunnelWeb.Web.Application.Mvc.Binders
{
    /// <summary>
    /// This binder lets us use conventions when we have a dictionary of different values. For example, we have Settings
    /// in the database which are just key/value pairs. We render them onto a form (via the <see cref="Features.Admin.AdminController"/>)
    /// as text boxes named with a prefix. When they are posted back, we receive something like: setting_Foo=1&setting_Bar=2. 
    /// This binder turns that into a dictionary of { Foo = 1, Bar = 2 }.
    /// </summary>
    public class DictionaryBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var dictionaryType = bindingContext.ModelType;
            var dictionaryInstance = Activator.CreateInstance(dictionaryType);
            var dictionary = (IDictionary)dictionaryInstance;
            
            // BindingContext.ValueProvider changed quite a bit, you have to pass-in the full prefix, not a prefix-prefix
            // the only way to do "wild-card" matching on the prefix is to look at the form posted collection
            var request = controllerContext.HttpContext.Request;
            var prefix = bindingContext.ModelName + "_";
            foreach (var field in request.Form.AllKeys.Where(x => x.StartsWith(prefix)))
            {
                dictionary.Add(field.Replace(prefix, string.Empty), request[field]);
            }

            return dictionaryInstance;
        }
    }
}
