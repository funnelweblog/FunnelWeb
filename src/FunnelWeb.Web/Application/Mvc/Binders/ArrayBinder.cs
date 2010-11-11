using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FunnelWeb.Web.Application.Mvc.Binders
{
    /// <summary>
    /// Lets us take an array of values (comma seperated) or duplicate values of the same name from HTTP input 
    /// and convert it to an array of integers. For example, foo=1,2,3 or foo=1&foo=2&foo=3. Used for things like 
    /// checkbox lists.
    /// </summary>
    public class ArrayBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var selectedIdentities = new List<int>();
            var formValues = controllerContext.HttpContext.Request.Form;
            foreach (var parameter in formValues)
            {
                var key = (string) parameter;
                if (!key.StartsWith(bindingContext.ModelName))
                    continue;

                var value = formValues[key] ?? string.Empty;
                var values = value.Split(',').Select(x => x.Trim());
                foreach (var item in values)
                {
                    var id = 0;
                    if (int.TryParse(item, out id))
                    {
                        selectedIdentities.Add(id);
                    }
                }
            }

            return selectedIdentities.ToArray();
        }
    }
}