using System.Web.Mvc;
using System;
using System.Collections;
using System.Linq;

namespace FunnelWeb.Web.Application.Binders
{
    public class DictionaryBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var dictionaryType = bindingContext.ModelType;
            var dictionaryInstance = Activator.CreateInstance(dictionaryType);
            var dictionary = (IDictionary)dictionaryInstance;
            
            //bindingContext.ValueProvider changed quite a bit, you have to pass-in the full prefix, not a prefix-prefix
            //the only way to do "wild-card" matching on the prefix is to look at the form posted collection
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
